using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class TargetShape: IShape
    {
        public ShapeType type;

        [Tooltip("Radius of the shape, not counting the central cell: a circle of diameter 7 would have a radius of 3")]
        public Vector2Int radius;
        
        public IEnumerable<Vector2Int> EvaluateAt(Vector2Int position)
        {
            return type switch
            {
                ShapeType.Square => Square(position, radius),
                ShapeType.Circle => Circle(position, radius),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<Vector2Int> Circle(Vector2Int position, Vector2Int radius)
        {
            return Shape.Circle(position, 2 * radius + Vector2Int.one, radius);
        }

        private static IEnumerable<Vector2Int> Square(Vector2Int position, Vector2Int radius)
        {
            return Shape.Square(position, 2 * radius + Vector2Int.one, radius);
        }
    }
}
