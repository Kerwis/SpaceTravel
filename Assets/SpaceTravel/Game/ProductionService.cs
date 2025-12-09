using System;
using System.Collections.Generic;

namespace SpaceTravel.Game
{
    public static class ProductionService
    {
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

            int safetyCounter = 0;
            const int maxCyclesPerTick = 10000;

            while (module.RecipeProgressSeconds >= cycleDuration)
            {
                if (safetyCounter++ > maxCyclesPerTick)
                    break;

                if (!StorageService.CanConsumeInputs(storage, recipe.Inputs))
                {
                    module.RecipeProgressSeconds = cycleDuration;
                    break;
                }

                if (!CanStoreOutputs(storage, recipe.Outputs, outputMult, defs))
                {
                    module.RecipeProgressSeconds = cycleDuration;
                    break;
                }

                module.RecipeProgressSeconds -= cycleDuration;

                StorageService.ConsumeInputs(storage, recipe.Inputs);
                ApplyOutputs(storage, recipe.Outputs, outputMult, defs);
            }
        }

        private static ModuleLevelStats GetLevelStats(ModuleDefinition def, int level)
        {
            if (def.Levels == null || def.Levels.Length == 0)
                return null;

            for (int i = 0; i < def.Levels.Length; i++)
            {
                if (def.Levels[i].Level == level)
                    return def.Levels[i];
            }

            return def.Levels[def.Levels.Length - 1];
        }

        private static bool CanStoreOutputs(StorageState storage, ResourceAmount[] outputs, float outputMult, IGameDefinitions defs)
        {
            if (outputs == null || outputs.Length == 0)
                return true;

            var requiredPerGroup = new Dictionary<ResourceGroup, float>();

            foreach (var outRes in outputs)
            {
                if (outRes == null || outRes.Amount <= 0f)
                    continue;

                var resDef = outRes.Resource;
                if (resDef == null)
                    continue;

                var group = resDef.Group;
                float amount = outRes.Amount * outputMult;

                if (!requiredPerGroup.ContainsKey(group))
                    requiredPerGroup[group] = 0f;

                requiredPerGroup[group] += amount;
            }

            foreach (var kvp in requiredPerGroup)
            {
                var group = kvp.Key;
                var required = kvp.Value;

                float capacity = storage.GetCapacity(group);
                float current = StorageService.GetTotalAmountForGroup(storage, group, defs);

                float free = capacity - current;
                if (free + 0.0001f < required)
                    return false;
            }

            return true;
        }

        private static void ApplyOutputs(StorageState storage, ResourceAmount[] outputs, float outputMult, IGameDefinitions defs)
        {
            if (outputs == null || outputs.Length == 0)
                return;

            foreach (var outRes in outputs)
            {
                if (outRes == null || outRes.Amount <= 0f)
                    continue;

                var resDef = outRes.Resource;
                if (resDef == null)
                    continue;

                float amount = outRes.Amount * outputMult;

                var group = resDef.Group;
                float capacity = storage.GetCapacity(group);
                float currentGroupAmount = StorageService.GetTotalAmountForGroup(storage, group, defs);
                float free = capacity - currentGroupAmount;

                if (free <= 0f)
                    continue;

                float toStore = amount;
                if (toStore > free)
                    toStore = free;

                var resourceId = resDef.Id;
                float current = storage.GetAmount(resourceId);
                storage.SetAmount(resourceId, current + toStore);
            }
        }
    }
}
