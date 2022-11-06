using GameEngine;
using GameEngine.Towers;
using Managers;
using UnityEngine;
using Utils.Extensions;

namespace UI
{
    public class UITowerMenuController : MonoBehaviour
    {
        public UITowerMenuButton buttonPrefab;

        private GameConfig _gameConfig;

        private void Start()
        {
            _gameConfig = GameManager.Instance.gameConfig;

            SpawnTowerIcons();
        }

        private void SpawnTowerIcons()
        {
            this.RemoveAllChildren();

            foreach (TowerConfig tower in _gameConfig.towers)
            {
                UITowerMenuButton button = Instantiate(buttonPrefab, transform);
                button.SetTower(tower);
            }
        }
    }
}
