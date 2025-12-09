using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class TileRow
    {
        [Tooltip("Columns for this row, indexed as [x].")]
        public TileType[] Columns;
    }
}