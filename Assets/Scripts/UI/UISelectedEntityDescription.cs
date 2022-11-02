using System.Collections.Generic;
using GameEngine.Processor;
using GameEngine.Towers;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils;
using Utils.CustomComponents;

namespace UI
{
    public class UISelectedEntityDescription : MyMonoBehaviour
    {
        public Transform selectedTowerRoot;
        public Transform inPreviewRoot;
        public Transform noSelectedTowerRoot;

        [Space(10)]
        public TextMeshProUGUI nameText;

        public TextMeshProUGUI kills;
        public TextMeshProUGUI resell;
        public UIGaugeController health;
        public UIGaugeController charge;

        [Space(10)]
        public int nPriorities;

        public Toggle priorityToggle;

        [Space(10)]
        public GameObject[] processorSpecific;

        public GameObject[] towerSpecific;

        private readonly List<Toggle> _toggles = new();

        void Start()
        {
            Assert.IsNotNull(selectedTowerRoot);
            Assert.IsNotNull(priorityToggle);

            RequireGameManager();

            for (int i = 1; i <= nPriorities; i++)
            {
                Toggle toggle = Instantiate(priorityToggle, priorityToggle.transform.parent);
                toggle.gameObject.SetActive(true);
                _toggles.Add(toggle);
                TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
                text.SetText(i.ToString());

                int localI = i;
                toggle.onValueChanged.AddListener(b =>
                {
                    if (b)
                    {
                        SetPriority(localI);
                    }
                });
            }

            priorityToggle.gameObject.SetActive(false);
        }

        void Update()
        {
            Description description = null;
            if (GameManager.SelectedEntity != null)
            {
                if (GameManager.SelectedEntity.IsTowerSelected())
                {
                    TowerState towerState = GameManager.SelectedEntity.GetSelectedTower();
                    description = Description.From(GameManager, towerState);
                    Display(false, true);
                    UpdatePriority(towerState);
                }
                else if (GameManager.SelectedEntity.IsProcessorSelected())
                {
                    ProcessorState processorState = GameManager.GameState.GetProcessorState();
                    description = Description.From(GameManager, processorState);
                    Display(true, false);
                }
            }

            if (description == null)
            {
                if (GameManager.TowerSpawnPreview?.InPreview ?? false)
                {
                    ShowPreviewRoot();
                }
                else
                {
                    Unselect();
                }
            }
            else
            {
                Select(description);
            }
        }

        public void Sell()
        {
            if (GameManager.SelectedEntity.IsTowerSelected())
            {
                TowerState towerState = GameManager.SelectedEntity.GetSelectedTower();
                GameManager.Tower.Sell(towerState);
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

            if (inPreviewRoot)
            {
                inPreviewRoot.gameObject.SetActive(false);
            }

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

            if (resell && state.Resell.HasValue)
            {
                resell.SetText(state.Resell.Value.ToString());
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

        private void ShowPreviewRoot()
        {
            selectedTowerRoot.gameObject.SetActive(false);

            if (inPreviewRoot)
            {
                inPreviewRoot.gameObject.SetActive(true);
            }

            if (noSelectedTowerRoot)
            {
                noSelectedTowerRoot.gameObject.SetActive(false);
            }
        }

        private void Unselect()
        {
            selectedTowerRoot.gameObject.SetActive(false);

            if (inPreviewRoot)
            {
                inPreviewRoot.gameObject.SetActive(false);
            }

            if (noSelectedTowerRoot)
            {
                noSelectedTowerRoot.gameObject.SetActive(true);
            }
        }

        private void SetPriority(int priority)
        {
            TowerState selectedTower = GameManager.SelectedEntity.GetSelectedTower();
            if (selectedTower == null)
            {
                return;
            }

            GameManager.Tower.SetPriority(selectedTower, priority);
        }

        private void UpdatePriority(TowerState towerState)
        {
            if (towerState.priority >= 1 && towerState.priority <= nPriorities)
            {
                _toggles[towerState.priority - 1].SetIsOnWithoutNotify(true);
            }
            else if (towerState.priority < 1)
            {
                _toggles[0].SetIsOnWithoutNotify(true);
            }
            else
            {
                _toggles[^1].SetIsOnWithoutNotify(true);
            }
        }

        private class Description
        {
            public string Name;
            public int? Kills;
            public int? Resell;
            public GaugeState? Health;
            public GaugeState Charge;

            public static Description From(GameManager gameManager, TowerState towerState)
            {
                if (towerState == null)
                {
                    return null;
                }

                return new Description
                {
                    Name = towerState.config ? towerState.config.towerName : "__TOWER__",
                    Kills = towerState.kills,
                    Resell = gameManager.Tower.SellValue(towerState),
                    Charge = towerState.charge,
                };
            }

            public static Description From(GameManager gameManager, ProcessorState processorState)
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
