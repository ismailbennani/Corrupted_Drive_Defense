using GameEngine.Towers;
using Managers;
using TMPro;
using UnityEngine.UI;
using Utils.CustomComponents;

namespace UI
{
    public class UITowerMenuButton: MyMonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;

        private TowerConfig _tower;

        void Start()
        {
            RequireGameManager();
        }
        
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

            if (!GameManager.Ready)
            {
                return;
            }

            GameManager.StartSpawning(_tower);
        }
    }
}
