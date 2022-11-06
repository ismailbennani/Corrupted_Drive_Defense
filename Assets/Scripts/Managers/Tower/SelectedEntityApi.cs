using System;
using System.Linq;
using GameEngine;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class SelectedEntityApi
    {
        private readonly GameConfig _gameConfig;
        private readonly VisibleShapeApi _visibleShape;
        private TowerState _selectedTower;
        private bool _processorIsSelected;

        public SelectedEntityApi(GameConfig gameConfig, VisibleShapeApi visibleShape)
        {
            Assert.IsNotNull(visibleShape);

            _gameConfig = gameConfig;
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
            tower.onUpgrade.AddListener(RefreshVisibleShapeOnUpgrade);

            ShowVisibleShape(tower);
        }

        public void SelectProcessor()
        {
            Clear();

            _processorIsSelected = true;
        }

        public void Clear()
        {
            _selectedTower?.onUpgrade.RemoveListener(RefreshVisibleShapeOnUpgrade);
            
            _selectedTower = null;
            _processorIsSelected = false;
            _visibleShape.Hide();
        }

        private void RefreshVisibleShapeOnUpgrade(int _)
        {
            ShowVisibleShape(_selectedTower);
        }

        private void ShowVisibleShape(TowerState tower)
        {
            switch (tower.description.targetType)
            {
                case TargetType.None:
                    _visibleShape.Show(null, tower.cells.Select(c => c.gridPosition), tower.rotated, _gameConfig.shapePreviewOkColor, false);
                    break;
                case TargetType.Single:
                case TargetType.AreaAtTarget:
                    _visibleShape.Show(tower.description.range, tower.cells.Select(c => c.gridPosition), tower.rotated, _gameConfig.shapePreviewOkColor, false);
                    break;
                case TargetType.AreaAtSelf:
                    _visibleShape.Show(tower.description.targetShape, tower.cells.Select(c => c.gridPosition), tower.rotated, _gameConfig.shapePreviewOkColor, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
