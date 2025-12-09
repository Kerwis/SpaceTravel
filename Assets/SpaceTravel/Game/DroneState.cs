using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class DroneState
    {
        [Tooltip("True if the combat drone feature is unlocked for the player.")]
        public bool Unlocked;

        [Tooltip("Normalized health value of the drone, 0 = destroyed, 1 = full health.")]
        public float Health = 1.0f;

        [Tooltip("Drone level (for vertical slice: single type, no equipment).")]
        public int Level = 1;
    }
}