using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ModuleLevelStats
    {
        [Tooltip("Module level (starts at 1).")]
        public int Level;

        [Tooltip("Energy consumption per second while the module is active.")]
        public float EnergyConsumptionPerSecond;

        [Tooltip("Multiplier applied to recipe BaseDurationSeconds. Lower is faster.")]
        public float SpeedMultiplier = 1.0f;

        [Tooltip("Optional efficiency multiplier applied to recipe outputs (1.0 = base amount).")]
        public float OutputMultiplier = 1.0f;
    }
}