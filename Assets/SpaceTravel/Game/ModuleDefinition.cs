using UnityEngine;

namespace SpaceTravel.Game
{
    [CreateAssetMenu(menuName = "SpaceTravel/ModuleDefinition")]
    public class ModuleDefinition : ScriptableObject
    {
        [Tooltip("Unique string identifier of the module type (e.g. 'reactor', 'mining_drill').")]
        public string Id;

        [Tooltip("Display name used in UI.")]
        public string DisplayName;

        [Tooltip("Optional high-level category (e.g. 'production', 'utility').")]
        public ModuleCategory Category;

        [Tooltip("Icon used in UI.")]
        public Sprite Icon;

        [Tooltip("Tile types where this module is allowed to be placed.")]
        public TileType[] AllowedTileTypes;
        
        [Tooltip("Cost to build this module.")]
        public ResourceAmount[] BuildCost;

        [Tooltip("If true, this module supports production overclock gameplay.")]
        public bool SupportsOverclock;

        [Tooltip("Production recipes that this module can execute.")]
        public ProductionRecipeDefinition[] Recipes;

        [Tooltip("Stats for each level of this module.")]
        public ModuleLevelStats[] Levels;
    }
}