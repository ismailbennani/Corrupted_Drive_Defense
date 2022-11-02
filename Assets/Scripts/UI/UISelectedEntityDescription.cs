﻿using GameEngine.Processor;
using GameEngine.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Utils.CustomComponents;

namespace UI
{
    public class UISelectedEntityDescription : MyMonoBehaviour
    {
        public Transform selectedTowerRoot;
        public Transform noSelectedTowerRoot;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI kills;
        public UIGaugeController health;
        public UIGaugeController charge;

        public GameObject[] processorSpecific;
        public GameObject[] towerSpecific;

        void Start()
        {
            Assert.IsNotNull(selectedTowerRoot);

            RequireGameManager();
        }

        void Update()
        {
            if (GameManager.SelectedEntity == null)
            {
                Unselect();
                return;
            }

            Description description = null;
            if (GameManager.SelectedEntity.IsTowerSelected())
            {
                TowerState towerState = GameManager.SelectedEntity.GetSelectedTower();
                description = Description.From(towerState);
                Display(false, true);
            } else if (GameManager.SelectedEntity.IsProcessorSelected())
            {
                ProcessorState processorState = GameManager.GameState.GetProcessorState();
                description = Description.From(processorState);
                Display(true, false);
            }

            if (description == null)
            {
                Unselect();
            }
            else
            {
                Select(description);
            }
        }

        private void Display(bool displayProcessorSpecific, bool displayTowerSpecific)
        {
            foreach (GameObject go in processorSpecific)
            {
                go.SetActive(displayProcessorSpecific);
            }
            
            foreach (GameObject go in towerSpecific)
            {
                go.SetActive(displayTowerSpecific);
            }
        }

        private void Select(Description state)
        {

            selectedTowerRoot.gameObject.SetActive(true);

            if (noSelectedTowerRoot)
            {
                noSelectedTowerRoot.gameObject.SetActive(false);
            }

            if (nameText)
            {
                nameText.SetText(state.Name);
            }

            if (kills && state.Kills.HasValue)
            {
                kills.SetText(state.Kills.Value.ToString());
            }

            if (health && state.Health.HasValue)
            {
                health.Set(state.Health.Value);
            }

            if (charge)
            {
                charge.Set(state.Charge);
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

        private class Description
        {
            public string Name;
            public int? Kills;
            public GaugeState? Health;
            public GaugeState Charge;

            public static Description From(TowerState towerState)
            {
                if (towerState == null)
                {
                    return null;
                }

                return new Description
                {
                    Name = towerState.config ? towerState.config.towerName : "__TOWER__",
                    Kills = towerState.kills,
                    Charge = towerState.charge,
                };
            }

            public static Description From(ProcessorState processorState)
            {
                if (processorState == null)
                {
                    return null;
                }
                
                return new Description
                {
                    Name = "Processor",
                    Health = processorState.health,
                    Charge = processorState.charge,
                };
            }
        }
    }
}