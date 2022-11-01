using System;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine;

namespace Managers.Tower
{
    public class SelectedTowerManager : MonoBehaviour
    {
        public TowerState selectedTower;
        public VisibleShapeManager visibleShapeManager;
        
        void Start()
        {
            if (!visibleShapeManager)
            {
                throw new InvalidOperationException("could not find visible shape manager");
            }
        }

        public void Select(TowerState tower)
        {
            selectedTower = tower;
            visibleShapeManager.Show(tower.config.targetArea, tower.cell.worldPosition);
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
            visibleShapeManager.Hide();
        }
    }
}
