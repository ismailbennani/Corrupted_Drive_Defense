using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEngine.Map
{
    public class GameMap: IEnumerable<Cell>
    {
        public Vector2Int Spawn => _fullPath[0];

        private readonly MapConfig _config;
        private readonly Vector2Int[] _fullPath;
        private readonly Cell[,] _cells;

        public GameMap(MapConfig config)
        {
            _config = config;
            _fullPath = ComputeFullPath(config);
            _cells = ComputeCells(config, _fullPath);
        }

        public Cell GetCellAt(Vector2Int cell)
        {
            if (IsInBounds(_config, cell))
            {
                Vector2Int arrayPosition = GetArrayPosition(_config, cell);
                return _cells[arrayPosition.x, arrayPosition.y];
            }

            return new Cell { type = CellType.Wall, gridPosition = cell };
        }

        public Cell GetCellFromLocalWorldPosition(Vector2 worldPosition)
        {
            Vector2Int cell = GetGridCellPosition(_config, worldPosition);
            return GetCellAt(cell);
        }

        public Vector2 GetLocalWorldPosition(Cell cell)
        {
            return cell.gridPosition + Vector2.one / 2;
        }

        private static Vector2Int[] ComputeFullPath(MapConfig mapConfig)
        {
            List<Vector2Int> result = new();

            for (int i = 0; i < mapConfig.path.Length - 1; i++)
            {
                Vector2Int cell = mapConfig.path[i];
                Vector2Int nextCell = mapConfig.path[i + 1];

                bool xChange = cell.x != nextCell.x;
                bool yChange = cell.y != nextCell.y;

                if (!xChange && !yChange)
                {
                    Debug.LogWarning($"Duplicate cell in path: {cell}");
                    continue;
                }

                if (xChange && yChange)
                {
                    throw new InvalidOperationException($"Path must be made of horizontal and vertical lines, found diagonal: {cell} -> {nextCell}");
                }

                int start = xChange ? cell.x : cell.y;
                int end = xChange ? nextCell.x : nextCell.y;

                for (int j = start; j <= end; j++)
                {
                    Vector2Int pathCell = xChange ? new Vector2Int(j, cell.y) : new Vector2Int(cell.x, j);

                    result.Add(pathCell);
                }
            }

            return result.ToArray();
        }

        private static Cell[,] ComputeCells(MapConfig mapConfig, IEnumerable<Vector2Int> fullPath)
        {
            Cell[,] result = new Cell[mapConfig.mapSize.y, mapConfig.mapSize.x];

            for (int x = 0; x < mapConfig.mapSize.x; x++)
            {
                for (int y = 0; y < mapConfig.mapSize.y; y++)
                {
                    Vector2Int cell = new(mapConfig.bottomLeftCorner.x + x, mapConfig.bottomLeftCorner.y + y);

                    result[y, x].gridPosition = cell;
                    result[y, x].type = CellType.Free;
                }
            }

            foreach (BoundsInt area in mapConfig.walls)
            {
                foreach (Vector3Int cell in area.allPositionsWithin)
                {
                    Vector2Int arrayPosition = GetArrayPosition(mapConfig, (Vector2Int)cell);
                    result[arrayPosition.x, arrayPosition.y].type = CellType.Wall;
                }
            }

            foreach (Vector2Int cell in fullPath)
            {
                Vector2Int arrayPosition = GetArrayPosition(mapConfig, cell);
                result[arrayPosition.x, arrayPosition.y].type = CellType.Path;
            }

            return result;
        }

        private static bool IsInBounds(MapConfig mapConfig, Vector2Int result)
        {
            return result.x >= mapConfig.bottomLeftCorner.x
                   && result.x < mapConfig.bottomLeftCorner.x + mapConfig.mapSize.x
                   && result.y >= mapConfig.bottomLeftCorner.y
                   && result.y < mapConfig.bottomLeftCorner.y + mapConfig.mapSize.y;
        }

        private static Vector2Int GetArrayPosition(MapConfig mapConfig, Vector2Int position)
        {
            return new Vector2Int(position.y - mapConfig.bottomLeftCorner.y, position.x - mapConfig.bottomLeftCorner.x);
        }

        private static Vector2Int GetGridCellPosition(MapConfig mapConfig, Vector2 position)
        {
            return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return _cells.Cast<Cell>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
