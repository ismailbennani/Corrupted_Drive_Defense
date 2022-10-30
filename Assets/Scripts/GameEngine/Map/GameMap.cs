using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Map
{
    public class GameMap
    {
        public Vector2Int Spawn => _fullPath[0];
        
        private readonly MapConfig _config;
        private readonly Vector2Int[] _fullPath;
        private readonly CellType[,] _mapCellTypes;

        public GameMap(MapConfig config)
        {
            _config = config;
            _fullPath = ComputeFullPath(config);
            _mapCellTypes = ComputeCellTypes(config, _fullPath);
        }

        public Vector2 GetGridCellPosition(Vector2Int cell)
        {
            return cell + Vector2.one / 2;
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

        private static CellType[,] ComputeCellTypes(MapConfig mapConfig, IEnumerable<Vector2Int> fullPath)
        {
            CellType[,] result = new CellType[mapConfig.mapSize.y, mapConfig.mapSize.x];

            foreach (BoundsInt area in mapConfig.walls)
            {
                foreach (Vector3Int cell in area.allPositionsWithin)
                {
                    Vector2Int arrayPosition = GetArrayPosition(mapConfig, (Vector2Int)cell);
                    result[arrayPosition.x, arrayPosition.y] = CellType.Wall;
                }
            }
            
            foreach (Vector2Int cell in fullPath)
            {
                Vector2Int arrayPosition = GetArrayPosition(mapConfig, cell);
                result[arrayPosition.x, arrayPosition.y] = CellType.Path;
            }

            return result;
        }

        private static Vector2Int GetArrayPosition(MapConfig mapConfig, Vector2Int position)
        {
            return new Vector2Int(mapConfig.topLeftCorner.y - position.y, position.x - mapConfig.topLeftCorner.x);
        }
    }
}
