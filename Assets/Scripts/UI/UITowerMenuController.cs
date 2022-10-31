using System;
using GameEngine;
using GameEngine.Tower;
using UnityEngine;
using Utils.Extensions;

namespace UI
{
    public class UITowerMenuController: MonoBehaviour
    {
        public UITowerMenuButton buttonPrefab;
        
        private GameConfig _gameConfig;

        void Start()
        {
            _gameConfig = GameManager.Instance.gameConfig;

            SpawnTowerIcons();
        }

        private void SpawnTowerIcons()
        {
            this.RemoveAllChildren();
            
            TowerConfig[] towers = { _gameConfig.capacitor };

            foreach (TowerConfig tower in towers)
            {
                UITowerMenuButton button = Instantiate(buttonPrefab, transform);
                button.SetTower(tower);
            }
        }
    }
}
