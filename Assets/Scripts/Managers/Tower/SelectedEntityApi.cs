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

            switch (tower.config.targetType)
            {
                case TargetType.Single:
                case TargetType.AreaAtTarget:
                    _visibleShape.Show(tower.config.range,
                                       tower.cells.Select(c => c.gridPosition),
                                       tower.rotated,
                                       _gameConfig.shapePreviewOkColor,
                                       aboveEntities: false);
                    break;
                case TargetType.AreaAtSelf:
                    _visibleShape.Show(tower.config.targetShape,
                                       tower.cells.Select(c => c.gridPosition),
                                       tower.rotated,
                                       _gameConfig.shapePreviewOkColor,
                                       aboveEntities: false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
