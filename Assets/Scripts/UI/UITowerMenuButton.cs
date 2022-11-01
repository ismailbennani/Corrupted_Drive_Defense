using GameEngine.Towers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITowerMenuButton: MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;

        private TowerConfig _tower;
        
        public void SetTower(TowerConfig tower)
        {
            nameText.text = tower.towerName;
            image.sprite = tower.sprite;

            _tower = tower;
        }

        public void Click()
        {
            if (!_tower)
            {
                return;
            }

            GameManager gameManager = GameManager.Instance;
            if (!gameManager)
            {
                return;
            }

            gameManager.StartSpawning(_tower);
        }
    }
}
