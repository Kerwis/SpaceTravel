using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class CrewState
    {
        [Tooltip("Normalized health of the main astronaut, 0 = dead, 1 = full health.")]
        public float Health = 1.0f;

        [Tooltip("Placeholder for radiation resistance. Used for future balancing.")]
        public float RadiationResistance = 1.0f;
    }
}