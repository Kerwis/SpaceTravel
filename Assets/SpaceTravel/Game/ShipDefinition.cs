using UnityEngine;

namespace SpaceTravel.Game
{
    [CreateAssetMenu(menuName = "SpaceTravel/ShipDefinition")]
    public class ShipDefinition : ScriptableObject
    {
        [Tooltip("Unique identifier of the ship definition.")]
        public string Id;

        [Tooltip("Ship tile layout.")]
        public TileGrid Layout;
    }
}