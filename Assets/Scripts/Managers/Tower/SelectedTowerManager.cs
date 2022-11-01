using System;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class SelectedTowerManager : MonoBehaviour
    {
        public VisibleShapeApi VisibleShape;
        
        public TowerState selectedTower;
        
        void Start()
        {
            Assert.IsNotNull(VisibleShape);
        }

        public void Select(TowerState tower)
        {
            selectedTower = tower;
            VisibleShape.Show(tower.config.targetArea, tower.cell.gridPosition);
        }

        public void Unselect(TowerState tower)
        {
            if (selectedTower.id == tower.id)
            {
                Clear();
            }
        }

        public void Clear()
        {
            selectedTower = null;
            VisibleShape.Hide();
        }
    }
}
