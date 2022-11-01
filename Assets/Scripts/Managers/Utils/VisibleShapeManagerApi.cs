using GameEngine.Shapes;
using UnityEngine;

namespace Managers.Utils
{
    public class VisibleShapeManagerApi
    {
        private readonly VisibleShapeManager _visibleShapeManager;

        public VisibleShapeManagerApi(VisibleShapeManager visibleShapeManager)
        {
            _visibleShapeManager = visibleShapeManager;
        }

        public void Show(Shape shape, Vector2Int position, Color? color = null)
        {
            _visibleShapeManager.Show(shape, position, color);
        }
        
        public void Hide()
        {
            _visibleShapeManager.Hide();
        }
        
        public void SetColor(Color color)
        {
            _visibleShapeManager.SetColor(color);
        }
        
        public void SetPosition(Vector2Int cell, bool aboveEntities = false)
        {
            _visibleShapeManager.SetPosition(cell, aboveEntities);
        }
    }
}
