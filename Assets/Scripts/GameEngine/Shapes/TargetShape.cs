using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEngine.Shapes
{
    [Serializable]
    public class TargetShape: IShape, ICloneable
    {
        public ShapeType type;

        [Tooltip("Radius of the shape, not counting the central cell: a circle of diameter 7 would have a radius of 3")]
        public Vector2Int radius;

        public IEnumerable<Vector2Int> EvaluateAt(Vector2Int[] positions, bool rotated)
        {
            Vector2Int actualRadius = rotated ? new Vector2Int(radius.y, radius.x) : radius;
            
            return type switch
            {
                ShapeType.Square => CellsInSquare(positions, actualRadius),
                ShapeType.Circle => CellsInCircle(positions, actualRadius),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static void Apply(TargetShape shape, TargetShapeModifier modifier)
        {
            if (modifier.changeShape)
            {
                shape.type = modifier.newShape;
            }

            shape.radius += modifier.additionalRadius;
        }

        public static IEnumerable<Vector2Int> CellsInCircle(Vector2Int position, Vector2Int radius)
        {
            return Shape.CellsInCircle(position, 2 * radius + Vector2Int.one, radius);
        }

        public static IEnumerable<Vector2Int> CellsInSquare(Vector2Int position, Vector2Int radius)
        {
            return Shape.CellsInSquare(position, 2 * radius + Vector2Int.one, radius);
        }

        public static IEnumerable<Vector2Int> CellsInCircle(IEnumerable<Vector2Int> positions, Vector2Int radius)
        {
            return positions.SelectMany(p => CellsInCircle(p, radius)).Distinct();
        }

        public static IEnumerable<Vector2Int> CellsInSquare(IEnumerable<Vector2Int> positions, Vector2Int radius)
        {
            return positions.SelectMany(p => CellsInSquare(p, radius)).Distinct();
        }
    }
}
