using UnityEngine;

namespace GameEngine.Map
{
    public static class CellExtensions
    {
        public static WorldCell WithWorldPosition(this Cell cell, Vector2 worldPosition)
        {
            return new WorldCell { gridPosition = cell.gridPosition, worldPosition = worldPosition, type = cell.type };
        }

        public static Cell WithoutWorldPosition(this WorldCell worldCell)
        {
            return new Cell { gridPosition = worldCell.gridPosition, type = worldCell.type };
        }
    }
}
