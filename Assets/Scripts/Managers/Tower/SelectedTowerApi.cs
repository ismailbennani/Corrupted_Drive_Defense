using GameEngine.Towers;
using Managers.Utils;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class SelectedTowerApi
    {
        private readonly VisibleShapeApi _visibleShape;
        private TowerState _selectedTower;

        public SelectedTowerApi(VisibleShapeApi visibleShape)
        {
            Assert.IsNotNull(visibleShape);
            
            _visibleShape = visibleShape;
        }

        public TowerState Get()
        {
            return _selectedTower;
        }

        public void Select(TowerState tower)
        {
            _selectedTower = tower;
            _visibleShape.Show(tower.config.targetArea, tower.cell.gridPosition);
        }

        public void Unselect(TowerState tower)
        {
            if (_selectedTower.id == tower.id)
            {
                Clear();
            }
        }

        public void Clear()
        {
            _selectedTower = null;
            _visibleShape.Hide();
        }
    }
}
