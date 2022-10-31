using GameEngine.Towers;
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
            costText.text = $"$ {tower.cost.ToString()}";
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
