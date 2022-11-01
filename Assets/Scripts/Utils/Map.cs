using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using UnityEngine;

namespace Utils
{
    public static class Map
    {
        public static MapConfig GetConfig()
        {
            return GameManager.Instance.mapManager.mapConfig;
        }

        public static WorldCell GetCellAt(Vector2Int vector2)
        {
            return GameManager.Instance.mapManager.GetCellAt(vector2);
        }
        
        public static WorldCell GetCellAt(Cell cell)
        {
            return GetCellAt(cell.gridPosition);
        }

        public static WorldCell GetCellFromWorldPosition(Vector2 position)
        {
            return GameManager.Instance.mapManager.GetCellFromWorldPosition(position);
        }

        public static IEnumerable<WorldCell> GetPath()
        {
            return GameManager.Instance.mapManager.GameMap.GetPath().Select(GetCellAt).ToArray();
        }

        public static IEnumerable<WorldCell> GetCellsOfType(CellType type)
        {
            return GameManager.Instance.mapManager.GameMap.Where(c => c.type == type).Select(GetCellAt);
        }
    }
}
