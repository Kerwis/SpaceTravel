using System;
using UnityEngine;

namespace SpaceTravel.Game
{
    [Serializable]
    public class TileGrid
    {
        [Tooltip("Grid width in tiles.")]
        public int Width;

        [Tooltip("Grid height in tiles.")]
        public int Height;

        [Tooltip("Rows of tiles, indexed as [y]. Each row contains columns [x].")]
        public TileRow[] Rows;

        public TileType GetTile(int x, int y)
        {
            return Rows[y].Columns[x];
        }

        public void SetTile(int x, int y, TileType value)
        {
            Rows[y].Columns[x] = value;
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}