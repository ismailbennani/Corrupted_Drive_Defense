using System.Linq;
using GameEngine.Map;
using UnityEngine;
using Utils;
using Utils.CustomComponents;

namespace EditorUtils
{
    public class EditorMapVizualizer : MyMonoBehaviour
    {
        public bool visualize;

        public void OnDrawGizmosSelected()
        {
            if (!visualize || !TryGetGameManager())
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
            MapConfig mapConfig = GameManager.Map.GetConfig();

            WorldCell bottomLeftCorner = GameManager.Map.GetCellAt(mapConfig.bottomLeftCorner);
            WorldCell topRightCorner = GameManager.Map.GetCellAt(mapConfig.bottomLeftCorner + mapConfig.mapSize);

            Vector2 center = (topRightCorner.worldPosition + bottomLeftCorner.worldPosition) / 2;
            Vector2 size = topRightCorner.worldPosition - bottomLeftCorner.worldPosition;

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(center, size);
        }

        private void DrawWalls()
        {
            foreach (WorldCell cell in GameManager.Map.GetCellsOfType(CellType.Wall))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(cell.worldPosition, 0.25f);
            }
        }

        private void DrawPath()
        {
            WorldCell[] path = GameManager.Map.GetPath().ToArray();

            for (int i = 0; i < path.Length - 1; i++)
            {
                WorldCell cell = path[i];
                WorldCell nextCell = path[i + 1];

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(cell.worldPosition, nextCell.worldPosition);
            }

            foreach (WorldCell cell in GameManager.Map.GetCellsOfType(CellType.Path))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(cell.worldPosition, 0.25f);
            }
        }
    }
}
