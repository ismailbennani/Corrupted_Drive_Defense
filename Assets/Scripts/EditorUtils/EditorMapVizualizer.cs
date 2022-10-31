using System.Linq;
using GameEngine.Map;
using UnityEngine;
using Utils.Interfaces;

namespace EditorUtils
{
    [ExecuteInEditMode]
    public class EditorMapVizualizer: MonoBehaviour, INeedsGameManager
    {
        public GameManager GameManager { get; set; }

        public bool enabled;
        
        private MapManager _mapManager;

        public void OnDrawGizmosSelected()
        {
            if (!enabled || !TryGetMapManager())
            {
                return;
            }

            Color initialColor = Gizmos.color;
            
            DrawMapBoundaries();
            DrawWalls();

            Gizmos.color = initialColor;
        }

        private void DrawMapBoundaries()
        {
            MapConfig mapConfig = _mapManager.mapConfig;
            
            WorldCell bottomLeftCorner = _mapManager.GetCellAt(mapConfig.bottomLeftCorner);
            WorldCell topRightCorner = _mapManager.GetCellAt(mapConfig.bottomLeftCorner + mapConfig.mapSize);

            Vector2 center = (topRightCorner.worldPosition + bottomLeftCorner.worldPosition) / 2;
            Vector2 size = topRightCorner.worldPosition - bottomLeftCorner.worldPosition;

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(center, size);
            Gizmos.color = Color.clear;
        }

        private void DrawWalls()
        {
            foreach (WorldCell cell in _mapManager.GameMap.Where(c => c.type == CellType.Wall).Select(c => _mapManager.GetCellAt(c.gridPosition)))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(cell.worldPosition, 0.5f);
                Gizmos.color = Color.clear;
            }
        }

        private bool TryGetMapManager()
        {
            if (!this.TryGetGameManager())
            {
                return false;
            }

            _mapManager = GameManager.mapManager;
            return _mapManager;
        }
    }
}
