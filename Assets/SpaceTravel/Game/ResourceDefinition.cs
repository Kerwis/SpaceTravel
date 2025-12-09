using UnityEngine;

namespace SpaceTravel.Game
{
    [CreateAssetMenu(menuName = "SpaceTravel/ResourceDefinition")]
    public class ResourceDefinition : ScriptableObject
    {
        [Tooltip("Unique identifier of the resource.")]
        public string Id;

        [Tooltip("Logical group used for storage capacity and balancing.")]
        public ResourceGroup Group;

        [Tooltip("Display name used in UI.")]
        public string DisplayName;

        [Tooltip("Icon used in UI.")]
        public Sprite Icon;

        [Tooltip("Optional: base value used for balancing, shops, etc.")]
        public float BaseValue;
    }
}