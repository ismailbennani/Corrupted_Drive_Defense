using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using GameEngine.Shapes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Utils
{
    public class VisibleShapeApi
    {
        private readonly VisibleShapeManager _visibleShapeManager;

        public VisibleShapeApi(VisibleShapeManager visibleShapeManager)
        {
            Assert.IsNotNull(visibleShapeManager);
            
            _visibleShapeManager = visibleShapeManager;
        }

        public void Show(IShape shape, Vector2Int position, Color? color = null, bool? aboveEntities = null)
        {
            _visibleShapeManager.Show(shape, color: color, aboveEntities: aboveEntities, position);
        }

        public void Show(IShape shape, IEnumerable<Vector2Int> positions, Color? color = null, bool? aboveEntities = null)
        {
            _visibleShapeManager.Show(shape, color: color, aboveEntities: aboveEntities, positions.ToArray());
        }

        public void Hide()
        {
            _visibleShapeManager.Hide();
        }

        public void SetColor(Color color)
        {
            _visibleShapeManager.SetColor(color);
        }

        public void SetPositions(params Vector2Int[] cells)
        {
            _visibleShapeManager.SetPositions(cells);
        }

        public void SetPositions(IEnumerable<Vector2Int> cells)
        {
            SetPositions(cells.ToArray());
        }
    }
}
