using System;
using System.Collections.Generic;

namespace SpaceTravel.Game
{
    public static class ProductionService
    {
        private static readonly ModuleLevelStats DefaultLevelStats = new()
        {
            Level = 1,
            SpeedMultiplier = 1.0f,
            OutputMultiplier = 1.0f
        };

        public static void TickProduction(GameState state, double seconds, float globalOutputMultiplier, bool applyOverclock, IGameDefinitions defs)
        {
            if (state.Ship == null || state.Ship.Modules == null)
                return;

            var modules = state.Ship.Modules;

            for (int i = 0; i < modules.Count; i++)
            {
                TickModule(modules[i], state.Storage, seconds, globalOutputMultiplier, applyOverclock, defs);
            }
        }

        private static void TickModule(
            ModuleInstanceState module,
            StorageState storage,
            double seconds,
            float globalOutputMultiplier,
            bool applyOverclock,
            IGameDefinitions defs)
        {
            if (module.Health <= 0f)
                return;

            var moduleDef = defs.GetModuleDefinition(module.Id);
            if (moduleDef == null || moduleDef.Recipes == null || moduleDef.Recipes.Length == 0)
                return;

            if (module.ActiveRecipeIndex < 0 || module.ActiveRecipeIndex >= moduleDef.Recipes.Length)
                return;

            var recipe = moduleDef.Recipes[module.ActiveRecipeIndex];
            if (recipe == null)
                return;

            var levelStats = GetLevelStats(moduleDef, module.Level);
            if (levelStats == null)
                return;

            float baseDuration = Math.Max(0.0001f, recipe.BaseDurationSeconds);
            float speedMult = Math.Max(0.0001f, levelStats.SpeedMultiplier);
            float cycleDuration = baseDuration * speedMult;

            float overclockMult = 1.0f;
            if (applyOverclock && moduleDef.SupportsOverclock && module.Overclock != null)
            {
                overclockMult += module.Overclock.Percent / 100f;
            }

            float outputMult = levelStats.OutputMultiplier * overclockMult * globalOutputMultiplier;

            module.RecipeProgressSeconds += (float)seconds;
            int cyclesAvailable = (int)Math.Floor(module.RecipeProgressSeconds / cycleDuration);
            if (cyclesAvailable <= 0)
                return;

            int maxByInputs = GetMaxCyclesForInputs(storage, recipe.Inputs);
            int maxByOutputs = GetMaxCyclesForOutputs(storage, recipe.Outputs, outputMult, defs);
            int cyclesToRun = Math.Min(cyclesAvailable, Math.Min(maxByInputs, maxByOutputs));

            if (cyclesToRun <= 0)
            {
                module.RecipeProgressSeconds = cycleDuration;
                return;
            }

            module.RecipeProgressSeconds -= cycleDuration * cyclesToRun;
            ApplyInputCycles(storage, recipe.Inputs, cyclesToRun);
            ApplyOutputCycles(storage, recipe.Outputs, outputMult, cyclesToRun);

            if (cyclesAvailable > cyclesToRun)
            {
                module.RecipeProgressSeconds = cycleDuration;
            }
        }

        private static ModuleLevelStats GetLevelStats(ModuleDefinition def, int level)
        {
            if (def.Levels == null || def.Levels.Length == 0)
                return DefaultLevelStats;

            for (int i = 0; i < def.Levels.Length; i++)
            {
                if (def.Levels[i].Level == level)
                    return def.Levels[i];
            }

            return def.Levels[def.Levels.Length - 1];
        }

        private static int GetMaxCyclesForInputs(StorageState storage, ResourceAmount[] inputs)
        {
            if (inputs == null || inputs.Length == 0)
                return int.MaxValue;

            int maxCycles = int.MaxValue;

            foreach (var input in inputs)
            {
                if (input == null || input.Amount <= 0f)
                    continue;

                if (input.Resource == null)
                    continue;

                var resourceId = input.Resource.Id;
                float current = storage.GetAmount(resourceId);
                int possible = (int)Math.Floor(current / input.Amount);
                if (possible < maxCycles)
                    maxCycles = possible;
            }

            return maxCycles;
        }

        private static int GetMaxCyclesForOutputs(StorageState storage, ResourceAmount[] outputs, float outputMult, IGameDefinitions defs)
        {
            if (outputs == null || outputs.Length == 0)
                return int.MaxValue;

            var perGroupPerCycle = new Dictionary<ResourceGroup, float>();

            foreach (var output in outputs)
            {
                if (output == null || output.Amount <= 0f)
                    continue;

                var resDef = output.Resource;
                if (resDef == null)
                    continue;

                float amount = output.Amount * outputMult;
                if (amount <= 0f)
                    continue;

                var group = resDef.Group;
                if (!perGroupPerCycle.ContainsKey(group))
                    perGroupPerCycle[group] = 0f;

                perGroupPerCycle[group] += amount;
            }

            int maxCycles = int.MaxValue;
            foreach (var kvp in perGroupPerCycle)
            {
                var group = kvp.Key;
                float perCycle = kvp.Value;
                if (perCycle <= 0f)
                    continue;

                float capacity = storage.GetCapacity(group);
                float current = StorageService.GetTotalAmountForGroup(storage, group, defs);
                float free = capacity - current;
                if (free <= 0f)
                    return 0;

                int possible = (int)Math.Floor(free / perCycle);
                if (possible < maxCycles)
                    maxCycles = possible;
            }

            return maxCycles;
        }

        private static void ApplyInputCycles(StorageState storage, ResourceAmount[] inputs, int cycles)
        {
            if (inputs == null || inputs.Length == 0)
                return;

            foreach (var input in inputs)
            {
                if (input == null || input.Amount <= 0f)
                    continue;

                if (input.Resource == null)
                    continue;

                var resourceId = input.Resource.Id;
                float current = storage.GetAmount(resourceId);
                storage.SetAmount(resourceId, current - input.Amount * cycles);
            }
        }

        private static void ApplyOutputCycles(StorageState storage, ResourceAmount[] outputs, float outputMult, int cycles)
        {
            if (outputs == null || outputs.Length == 0)
                return;

            foreach (var output in outputs)
            {
                if (output == null || output.Amount <= 0f)
                    continue;

                var resDef = output.Resource;
                if (resDef == null)
                    continue;

                float amount = output.Amount * outputMult * cycles;

                var resourceId = resDef.Id;
                float current = storage.GetAmount(resourceId);
                storage.SetAmount(resourceId, current + amount);
            }
        }
    }
}
