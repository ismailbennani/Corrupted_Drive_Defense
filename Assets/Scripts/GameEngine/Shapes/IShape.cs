using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Shapes
{
    public interface IShape
    {
        IEnumerable<Vector2Int> EvaluateAt(Vector2Int[] positions, bool rotated);
    }
}
