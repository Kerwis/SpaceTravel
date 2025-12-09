using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class PlayerProgressionState
    {
        [Tooltip("Overall player level or account level.")]
        public int Level;

        [Tooltip("Total amount of gold earned over the whole account lifetime.")]
        public long TotalGoldEarned;

        [Tooltip("Total number of PvP battles played.")]
        public long PvpBattlesPlayed;
    }
}