using System;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine;

namespace Managers.Tower
{
    public class SelectedTowerManager : MonoBehaviour
    {
        public VisibleShapeManagerApi VisibleShapeManager;
        
        public TowerState selectedTower;
        
        void Start()
        {
            if (VisibleShapeManager == null)
            {
                throw new InvalidOperationException("could not find visible shape manager");
            }
        }

        public void Select(TowerState tower)
        {
            selectedTower = tower;
            VisibleShapeManager.Show(tower.config.targetArea, tower.cell.gridPosition);
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
            VisibleShapeManager.Hide();
        }
    }
}
