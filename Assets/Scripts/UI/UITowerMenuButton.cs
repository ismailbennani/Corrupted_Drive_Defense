using GameEngine.Tower;
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
        
        public void SetTower(TowerConfig tower)
        {
            nameText.text = tower.name;
            costText.text = $"$ {tower.cost.ToString()}";
            image.sprite = tower.sprite;
        }
    }
}
