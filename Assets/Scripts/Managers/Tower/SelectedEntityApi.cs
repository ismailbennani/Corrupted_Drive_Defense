using System.Linq;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class SelectedEntityApi
    {
        private readonly VisibleShapeApi _visibleShape;
        private TowerState _selectedTower;
        private bool _processorIsSelected;

        public SelectedEntityApi(VisibleShapeApi visibleShape)
        {
            Assert.IsNotNull(visibleShape);
            
            _visibleShape = visibleShape;
        }

        public TowerState GetSelectedTower()
        {
            return _selectedTower;
        }

        public bool IsTowerSelected()
        {
            return _selectedTower != null;
        }
        
        public bool IsProcessorSelected()
        {
            return _processorIsSelected;
        }

        public void Select(TowerState tower)
        {
            Clear();
            
            _selectedTower = tower;
            _visibleShape.Show(tower.config.range, tower.cells.Select(c => c.gridPosition), tower.rotated);
        }

        public void SelectProcessor()
        {
            Clear();
            
            _processorIsSelected = true;
        }

        public void Clear()
        {
            _selectedTower = null;
            _processorIsSelected = false;
            _visibleShape.Hide();
        }
    }
}
