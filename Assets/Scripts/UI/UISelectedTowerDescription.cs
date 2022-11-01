using System;
using GameEngine.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Utils.CustomComponents;

namespace UI
{
    public class UISelectedTowerDescription : MyMonoBehaviour
    {
        public Transform selectedTowerRoot;
        public Transform noSelectedTowerRoot;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI kills;
        public UIGaugeController ticks;

        void Start()
        {
            Assert.IsNotNull(selectedTowerRoot);
            
            RequireGameManager();
        }

        void Update()
        {
            if (GameManager.SelectedTower == null)
            {
                Unselect();
                return;
            }
            
            TowerState state = GameManager.SelectedTower.Get();

            if (state == null)
            {
                Unselect();
                return;
            }

            Select(state);
        }

        private void Select(TowerState state)
        {

            selectedTowerRoot.gameObject.SetActive(true);

            if (noSelectedTowerRoot)
            {
                noSelectedTowerRoot.gameObject.SetActive(false);
            }

            if (nameText)
            {
                nameText.SetText(state.config.towerName);
            }

            if (kills)
            {
                kills.SetText(state.kills.ToString());
            }

            if (ticks)
            {
                ticks.Set(state.ticks);
            }
        }

        private void Unselect()
        {

            selectedTowerRoot.gameObject.SetActive(false);

            if (noSelectedTowerRoot)
            {
                noSelectedTowerRoot.gameObject.SetActive(true);
            }
        }
    }
}
