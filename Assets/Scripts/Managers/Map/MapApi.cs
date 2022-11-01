using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Map
{
    public class MapApi
    {
        private readonly MapManager _mapManager;

        public MapApi(MapManager mapManager)
        {
            Assert.IsNotNull(mapManager);
            
            _mapManager = mapManager;
        }

        public MapConfig GetConfig()
        {
            return _mapManager.MapConfig;
        }

        public WorldCell GetCellAt(Vector2Int vector2)
        {
            return _mapManager.GetCellAt(vector2);
        }
        
        public WorldCell GetCellAt(Cell cell)
        {
            return GetCellAt(cell.gridPosition);
        }

        public WorldCell GetCellFromWorldPosition(Vector2 position)
        {
            return _mapManager.GetCellFromWorldPosition(position);
        }

        public IEnumerable<WorldCell> GetPath()
        {
            return _mapManager.GameMap.GetPath().Select(GetCellAt).ToArray();
        }

        public IEnumerable<WorldCell> GetCellsOfType(CellType type)
        {
            return _mapManager.GameMap.Where(c => c.type == type).Select(GetCellAt);
        }
    }
}
