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
                ShapeType.Square => Square(position),
                ShapeType.Circle => Circle(position),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private IEnumerable<Vector2Int> Square(Vector2Int position)
        {
            for (int i = -size; i <= size; i++)
            {
                for (int j = -size; j <= size; j++)
                {
                    yield return new Vector2Int(i + position.x, j + position.y);
                }
            }
        }

        private IEnumerable<Vector2Int> Circle(Vector2Int position)
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
