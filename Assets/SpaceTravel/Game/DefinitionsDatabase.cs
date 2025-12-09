using System.Collections.Generic;
using UnityEngine;

namespace SpaceTravel.Game
{
    public class DefinitionsDatabase : MonoBehaviour, IGameDefinitions
    {
        [Header("Definitions")]
        [Tooltip("All module definitions available in the game.")]
        public ModuleDefinition[] ModuleDefinitions;

        [Tooltip("All ship definitions available in the game.")]
        public ShipDefinition[] ShipDefinitions;

        [Tooltip("All resource definitions available in the game.")]
        public ResourceDefinition[] ResourceDefinitions;

        private readonly Dictionary<string, ModuleDefinition> _modulesById = new();
        private readonly Dictionary<string, ShipDefinition> _shipsById = new();
        private readonly Dictionary<string, ResourceDefinition> _resourcesById = new();

        private void Awake()
        {
            _modulesById.Clear();
            _shipsById.Clear();
            _resourcesById.Clear();

            if (ModuleDefinitions != null)
            {
                foreach (var def in ModuleDefinitions)
                {
                    if (def == null) continue;
                    _modulesById[def.Id] = def;
                }
            }

            if (ShipDefinitions != null)
            {
                foreach (var def in ShipDefinitions)
                {
                    if (def == null) continue;
                    if (!string.IsNullOrEmpty(def.Id))
                        _shipsById[def.Id] = def;
                }
            }

            if (ResourceDefinitions != null)
            {
                foreach (var def in ResourceDefinitions)
                {
                    if (def == null) continue;
                    _resourcesById[def.Id] = def;
                }
            }
        }

        public ModuleDefinition GetModuleDefinition(string id)
        {
            _modulesById.TryGetValue(id, out var def);
            return def;
        }

        public ShipDefinition GetShipDefinition(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            _shipsById.TryGetValue(id, out var def);
            return def;
        }

        public ResourceDefinition GetResourceDefinition(string id)
        {
            _resourcesById.TryGetValue(id, out var def);
            return def;
        }

        public IReadOnlyList<ModuleDefinition> GetAllModules()
        {
            return ModuleDefinitions;
        }

        public IReadOnlyList<ResourceDefinition> GetAllResources()
        {
            return ResourceDefinitions;
        }
    }
}
