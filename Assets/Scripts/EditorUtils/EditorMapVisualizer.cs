using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace EditorUtils
{
    public class EditorMapVisualizer : MyMonoBehaviour
    {
        public bool visualize;

        public void OnDrawGizmosSelected()
        {
            if (!visualize || !TryGetGameManager() || GameManager.Map == null)
            {
                return;
            }

            Color initialColor = Gizmos.color;

            DrawMapBoundaries();
            DrawWalls();
            DrawPath();
            DrawProcessor();
            DrawTowers();

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
            DrawCells(Color.red, GameManager.Map.GetCellsOfType(CellType.Wall).ToArray());
        }

        private void DrawPath()
        {
            DrawLine(Color.blue, GameManager.Map.GetPath().ToArray());
            DrawCells(Color.blue, GameManager.Map.GetCellsOfType(CellType.Path).ToArray());
        }

        private void DrawProcessor()
        {
            ProcessorState processorState = GameManager.GameState?.GetProcessorState();
            if (processorState == null)
            {
                return;
            }

            DrawCells(Color.gray, processorState.cells);
        }

        private void DrawTowers()
        {
            IEnumerable<TowerState> towerStates = GameManager.GameState?.GetTowers();
            if (towerStates == null)
            {
                return;
            }

            foreach (TowerState towerState in towerStates)
            {
                DrawCells(Color.gray, towerState.cells);
            }
        }

        private static void DrawCells(Color color, params WorldCell[] cells)
        {
            foreach (WorldCell cell in cells)
            {
                Gizmos.color = color;
                Gizmos.DrawWireSphere(cell.worldPosition, 0.25f);
            }
        }

        private static void DrawLine(Color color, params WorldCell[] path)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                WorldCell cell = path[i];
                WorldCell nextCell = path[i + 1];

                Gizmos.color = color;
                Gizmos.DrawLine(cell.worldPosition, nextCell.worldPosition);
            }
        }
    }
}
