using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class Shape
    {
        public ShapeType type;
        public int size;

        public IEnumerable<Vector2Int> EvaluateAt(Vector2Int position)
        {
            return type switch
            {
                ShapeType.Square => Square(position, size),
                ShapeType.Circle => Circle(position, size),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static IEnumerable<Vector2Int> Square(Vector2Int position, int size)
        {
            return Square(position, new Vector2Int(2 * size + 1, 2 * size + 1), new Vector2Int(size, size));
        }

        public static IEnumerable<Vector2Int> Square(Vector2Int position, Vector2Int size, Vector2Int offset)
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    yield return new Vector2Int(i + position.x - offset.x, j + position.y - offset.y);
                }
            }
        }

        public static IEnumerable<Vector2Int> Circle(Vector2Int position, int size)
        {
            for (int i = -size; i <= size; i++)
            {
                int height = size - Math.Abs(i);
                for (int j = -height; j <= height; j++)
                {
                    yield return new Vector2Int(i + position.x, j + position.y);
                }
            }
        }
    }
}
