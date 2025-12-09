using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class GameState
    {
        [Tooltip("Last time the game simulation was executed (UTC, in Unix seconds).")]
        public long LastSimUnixSeconds;

        [Tooltip("Current ship layout and module instances.")]
        public ShipState Ship;

        [Tooltip("Current storage amounts and capacities.")]
        public StorageState Storage;

        [Tooltip("Current drone progression state.")]
        public DroneState Drone;

        [Tooltip("Current crew state.")]
        public CrewState Crew;

        [Tooltip("Player progression meta data.")]
        public PlayerProgressionState Progress;
    }
}