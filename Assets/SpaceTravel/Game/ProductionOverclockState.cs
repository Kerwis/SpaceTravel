using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ProductionOverclockState
    {
        [Tooltip("Current overclock percentage in range 0-100.")]
        public float Percent;

        [Tooltip("If true, player input for overclock minigame is temporarily locked.")]
        public bool InputLocked;

        [Tooltip("Remaining time (in seconds) until input lock is removed.")]
        public float InputLockTimer;
    }
}