using UnityEngine;

namespace SpaceTravel.Game
{
    [CreateAssetMenu(menuName = "SpaceTravel/ProductionRecipe")]
    public class ProductionRecipeDefinition : ScriptableObject
    {
        [Tooltip("Unique identifier of this recipe.")]
        public string Id;

        [Tooltip("Display name used in UI.")]
        public string DisplayName;

        [Tooltip("Input resources consumed when this recipe runs once.")]
        public ResourceAmount[] Inputs;

        [Tooltip("Output resources produced when this recipe runs once.")]
        public ResourceAmount[] Outputs;

        [Tooltip("Base duration (in seconds) for one production cycle at speed multiplier = 1.0.")]
        public float BaseDurationSeconds = 1.0f;
    }
}