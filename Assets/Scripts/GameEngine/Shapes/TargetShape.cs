using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class TargetShape: IShape
    {
        public ShapeType type;

        [Tooltip("Radius of the shape, not counting the central cell: a circle of diameter 7 would have a radius of 3")]
        public Vector2Int radius;

        public IEnumerable<Vector2Int> EvaluateAt(Vector2Int[] positions, bool rotated)
        {
            Vector2Int actualRadius = rotated ? new Vector2Int(radius.y, radius.x) : radius;
            
            return type switch
            {
                ShapeType.Square => Square(positions, actualRadius),
                ShapeType.Circle => Circle(positions, actualRadius),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static IEnumerable<Vector2Int> Circle(Vector2Int position, Vector2Int radius)
        {
            return Shape.Circle(position, 2 * radius + Vector2Int.one, radius);
        }

        public static IEnumerable<Vector2Int> Square(Vector2Int position, Vector2Int radius)
        {
            return Shape.Square(position, 2 * radius + Vector2Int.one, radius);
        }

        public static IEnumerable<Vector2Int> Circle(IEnumerable<Vector2Int> positions, Vector2Int radius)
        {
            return positions.SelectMany(p => Circle(p, radius)).Distinct();
        }

        public static IEnumerable<Vector2Int> Square(IEnumerable<Vector2Int> positions, Vector2Int radius)
        {
            return positions.SelectMany(p => Square(p, radius)).Distinct();
        }
    }
}
