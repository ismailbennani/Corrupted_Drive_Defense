using System;
using UnityEngine;

namespace GameEngine.Map
{
    [Serializable]
    public struct WorldCell
    {
        public Vector2Int gridPosition;
        public Vector2 worldPosition;
        public CellType type;
    }
}
