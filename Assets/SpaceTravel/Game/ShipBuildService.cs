using UnityEngine;

namespace SpaceTravel.Game
{
    public static class ShipBuildService
    {
        public static bool TryBuildModuleAt(
            GameState state,
            IGameDefinitions defs,
            string moduleId,
            int x,
            int y,
            TileType tileType)
        {
            if (state == null || defs == null || state.Ship == null || state.Storage == null)
                return false;

            var ship = state.Ship;

            for (int i = 0; i < ship.Modules.Count; i++)
            {
                var m = ship.Modules[i];
                if (m.TilePos.x == x && m.TilePos.y == y)
                    return false;
            }

            var def = defs.GetModuleDefinition(moduleId);
            if (def == null)
                return false;

            if (def.AllowedTileTypes != null && def.AllowedTileTypes.Length > 0)
            {
                bool allowed = false;
                for (int i = 0; i < def.AllowedTileTypes.Length; i++)
                {
                    if (def.AllowedTileTypes[i] == tileType)
                    {
                        allowed = true;
                        break;
                    }
                }

                if (!allowed)
                    return false;
            }

            var cost = def.BuildCost;
            if (!StorageService.CanConsumeInputs(state.Storage, cost))
                return false;

            StorageService.ConsumeInputs(state.Storage, cost);

            var instance = new ModuleInstanceState
            {
                Id = def.Id,
                TilePos = new Vector2Int(x, y),
                Level = 1,
                Health = 1f,
                ActiveRecipeIndex = 0,
                RecipeProgressSeconds = 0f,
                Overclock = def.SupportsOverclock ? new ProductionOverclockState() : null
            };

            ship.Modules.Add(instance);

            return true;
        }
    }
}