using GameEngine.Towers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils.CustomComponents;

namespace UI
{
    public class UITowerMenuButton: MyMonoBehaviour
    {
        public Button button;
        public Image image;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;

        private TowerConfig _tower;

        void Start()
        {
            Assert.IsTrue(button);
            
            RequireGameManager();
        }

        void Update()
        {
            button.interactable = GameManager.GameState?.CanSpend(_tower.cost) ?? false;
        }
        
        public void SetTower(TowerConfig tower)
        {
            if (nameText)
            {
                nameText.SetText(tower.towerName);
            }

            if (costText)
            {
                costText.SetText(tower.cost.ToString());
            }

            if (image)
            {
                image.sprite = tower.sprite;
                image.color = Color.white;
            }

            _tower = tower;
        }

        public void Click()
        {
            if (!_tower)
            {
                return;
            }

            if (!GameManager.Ready)
            {
                return;
            }

            GameManager.StartSpawning(_tower);
        }
    }
}
