using System.Collections.Generic;
using System.Linq;
using GameEngine.Shapes;
using GameEngine.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils.CustomComponents;

namespace UI
{
    public class UITowerMenuButton : MyMonoBehaviour
    {
        public Button button;
        public GridLayoutGroup root;
        public Image towerImage;
        public List<Image> cells = new();

        [Header("Overlay")]
        public TextMeshProUGUI nameText;

        public TextMeshProUGUI costText;

        private TowerConfig _tower;

        private void Start()
        {
            Assert.IsNotNull(button);
            Assert.IsNotNull(root);
            Assert.IsNotNull(towerImage);
            Assert.IsTrue(cells?.Count > 0);
        }

        private void Update()
        {
            button.interactable = GameManager.GameState?.CanSpend(_tower.cost) ?? false;
            towerImage.rectTransform.sizeDelta = towerImage.sprite.bounds.size * cells[0].rectTransform.rect.size;
        }

        public void SetTower(TowerConfig tower)
        {
            Assert.IsNotNull(tower);

            root.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            root.constraintCount = tower.shape.size.x;
            SetCellPositions(tower.shape.EvaluateAt(Vector2Int.zero, false).ToArray());

            if (tower.sprite)
            {
                towerImage.sprite = tower.sprite;
                towerImage.color = Color.white;
            }

            if (nameText)
            {
                nameText.SetText(tower.towerName);
            }

            if (costText)
            {
                costText.SetText(tower.cost.ToString());
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

        private void SetCellPositions(Vector2Int[] positions)
        {
            for (int i = cells.Count; i < positions.Length; i++)
            {
                Image image = Instantiate(cells[0], root.transform);
                image.transform.SetAsFirstSibling();
                image.color = Color.white;

                cells.Add(image);
            }

            for (int i = 0; i < positions.Length; i++)
            {
                cells[i].gameObject.SetActive(true);
            }

            for (int i = positions.Length; i < cells.Count; i++)
            {
                cells[i].gameObject.SetActive(false);
            }
        }
    }
}
