using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class ModuleInstanceState
    {
        [Tooltip("Module type identifier.")]
        public string Id;

        [Tooltip("Grid position (tile coordinates) of this module on the ship.")]
        public Vector2Int TilePos;

        [Tooltip("Current level of this module (starts at 1).")]
        public int Level;

        [Tooltip("Normalized health value, 0 = destroyed, 1 = full health.")]
        public float Health = 1.0f;

        [Tooltip("Index of the active recipe in the module's recipe list.")]
        public int ActiveRecipeIndex;

        [Tooltip("Accumulated time in seconds for the current recipe cycle.")]
        public float RecipeProgressSeconds;

        [Tooltip("Per-machine production overclock state. Used only if module supports overclock.")]
        public ProductionOverclockState Overclock;
    }
}