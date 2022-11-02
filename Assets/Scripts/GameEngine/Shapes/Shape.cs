using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class Shape: IShape
    {
        public ShapeType type;
        public Vector2Int size;
        public Vector2Int offset;

        public IEnumerable<Vector2Int> EvaluateAt(Vector2Int position)
        {
            return type switch
            {
                ShapeType.Square => Square(position, size, offset),
                ShapeType.Circle => Circle(position, size, offset),
                _ => throw new ArgumentOutOfRangeException()
            };
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

        public static IEnumerable<Vector2Int> Circle(Vector2Int position, Vector2Int size, Vector2Int offset)
        {
            int halfSizeX = Mathf.FloorToInt(size.x / 2);
            int halfSizeY = Mathf.FloorToInt(size.y / 2);
            
            for (int i = 0; i < size.x; i++)
            {
                int height = halfSizeY - Mathf.CeilToInt(Math.Abs(i - halfSizeX) * halfSizeY / halfSizeX);
                int startY = halfSizeY - height;
                int endY = halfSizeY + height;
                
                for (int j = startY; j <= endY; j++)
                {
                    yield return new Vector2Int(i + position.x - offset.x, j + position.y - offset.y);
                }
            }
        }
    }
}
