using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class Shape : IShape
    {
        public ShapeType type;
        public Vector2Int size;
        public Vector2Int offset;

        public IEnumerable<Vector2Int> EvaluateAt(params Vector2Int[] positions)
        {
            return type switch
            {
                ShapeType.Square => Square(positions, size, offset),
                ShapeType.Circle => Circle(positions, size, offset),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static IEnumerable<Vector2Int> Square(Vector2Int position, Vector2Int size, Vector2Int offset)
        {
            if (size.x == 0 || size.y == 0)
            {
                yield break;
            }

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    yield return new Vector2Int(i + position.x - offset.x, j + position.y - offset.y);
                }
            }
        }

        public static IEnumerable<Vector2Int> Circle(Vector2Int position, Vector2Int size, Vector2Int offset)
        {
            if (size.x == 0 || size.y == 0)
            {
                yield break;
            }

            int halfSizeX = Mathf.FloorToInt(size.x / 2);
            int halfSizeY = Mathf.FloorToInt(size.y / 2);

            for (int i = 0; i < size.x; i++)
            {
                int height = halfSizeX == 0 ? halfSizeY : halfSizeY - Mathf.CeilToInt(Math.Abs(i - halfSizeX) * halfSizeY / halfSizeX);
                int startY = halfSizeY - height;
                int endY = halfSizeY + height;

                for (int j = startY; j <= endY; j++)
                {
                    yield return new Vector2Int(i + position.x - offset.x, j + position.y - offset.y);
                }
            }
        }

        public static IEnumerable<Vector2Int> Circle(IEnumerable<Vector2Int> positions, Vector2Int size, Vector2Int offset)
        {
            return positions.SelectMany(p => Circle(p, size, offset)).Distinct();
        }

        public static IEnumerable<Vector2Int> Square(IEnumerable<Vector2Int> positions, Vector2Int size, Vector2Int offset)
        {
            return positions.SelectMany(p => Square(p, size, offset)).Distinct();
        }
    }
}
