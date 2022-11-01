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
        public UIGaugeController charge;

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

            if (state?.config == null)
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

            if (charge)
            {
                charge.Set(state.charge);
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
