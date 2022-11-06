using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Shapes
{
    public static class ShapeExtensions
    {
        public static IEnumerable<Vector2Int> EvaluateAt(this IShape shape, Vector2Int position, bool rotated)
        {
            return shape.EvaluateAt(new[] { position }, rotated);
        }

        public static IEnumerable<Vector2Int> EvaluateAt(this IShape shape, params Vector2Int[] positions)
        {
            return shape.EvaluateAt(positions, false);
        }
    }
}
