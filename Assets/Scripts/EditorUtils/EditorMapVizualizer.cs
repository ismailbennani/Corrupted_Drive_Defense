using System.Linq;
using GameEngine.Map;
using UnityEngine;
using Utils.CustomComponents;

namespace EditorUtils
{
    public class EditorMapVizualizer : MyMonoBehaviour
    {
        public bool visualize;

        public void OnDrawGizmosSelected()
        {
            if (!visualize || !TryGetGameManager() || !GameManager.mapManager)
            {
                return;
            }

            Color initialColor = Gizmos.color;

            DrawMapBoundaries();
            DrawWalls();
            DrawPath();

            Gizmos.color = initialColor;
        }

        private void DrawMapBoundaries()
        {
            MapConfig mapConfig = GameManager.mapManager.mapConfig;

            WorldCell bottomLeftCorner = GameManager.mapManager.GetCellAt(mapConfig.bottomLeftCorner);
            WorldCell topRightCorner = GameManager.mapManager.GetCellAt(mapConfig.bottomLeftCorner + mapConfig.mapSize);

            Vector2 center = (topRightCorner.worldPosition + bottomLeftCorner.worldPosition) / 2;
            Vector2 size = topRightCorner.worldPosition - bottomLeftCorner.worldPosition;

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(center, size);
        }

        private void DrawWalls()
        {
            foreach (WorldCell cell in GameManager.mapManager.GameMap.Where(c => c.type == CellType.Wall).Select(c => GameManager.mapManager.GetCellAt(c.gridPosition)))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(cell.worldPosition, 0.25f);
            }
        }

        private void DrawPath()
        {
            Vector2Int[] path = GameManager.mapManager.mapConfig.path;

            for (int i = 0; i < path.Length - 1; i++)
            {
                WorldCell cell = GameManager.mapManager.GetCellAt(path[i]);
                WorldCell nextCell = GameManager.mapManager.GetCellAt(path[i + 1]);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(cell.worldPosition, nextCell.worldPosition);
            }

            foreach (WorldCell cell in GameManager.mapManager.GameMap.Where(c => c.type == CellType.Path).Select(c => GameManager.mapManager.GetCellAt(c.gridPosition)))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(cell.worldPosition, 0.25f);
            }
        }
    }
}
