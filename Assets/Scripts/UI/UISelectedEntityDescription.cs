using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Processor;
using GameEngine.Towers;
using Managers;
using TMPro;
using UI.DataStructures;
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
        public TMP_Dropdown strategiesDropdown;

        [Space(10)]
        public Transform upgradePathRoot;
        public UIUpgradePath upgradePathPrefab;

        [Space(10)]
        public GameObject[] processorSpecific;
        public GameObject[] towerSpecific;
        public GameObject[] strategySpecific;

        private TargetStrategy[] _lastStrategies = Array.Empty<TargetStrategy>();
        private readonly List<Toggle> _toggles = new();
        private List<UIUpgradePath> _upgradePaths = new();
        private List<List<UIUpgrade>> _upgrades = new();

        void Start()
        {
            Assert.IsNotNull(selectedTowerRoot);
            Assert.IsNotNull(priorityToggle);
            Assert.IsNotNull(strategiesDropdown);

            RequireGameManager();

            for (int i = 1; i <= nPriorities; i++)
            {
                Toggle toggle = Instantiate(priorityToggle, priorityToggle.transform.parent);
                toggle.gameObject.SetActive(true);
                _toggles.Add(toggle);
                TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
                text.SetText(i.ToString());

                int localI = i;
                toggle.onValueChanged.AddListener(
                    b =>
                    {
                        if (b)
                        {
                            SetPriority(localI);
                        }
                    }
                );
            }

            priorityToggle.gameObject.SetActive(false);
            upgradePathPrefab.gameObject.SetActive(false);
            strategiesDropdown.onValueChanged.AddListener(SetStrategy);
        }

        void Update()
        {
            if (GameManager.SelectedEntity == null)
            {
                return;
            }

            Description description = null;
            if (GameManager.SelectedEntity.IsTowerSelected())
            {
                TowerState towerState = GameManager.SelectedEntity.GetSelectedTower();
                description = Description.From(GameManager, towerState);
                Display(false, true, towerState.availableStrategies.Any());
            }
            else if (GameManager.SelectedEntity.IsProcessorSelected())
            {
                ProcessorState processorState = GameManager.GameState.GetProcessorState();
                description = Description.From(GameManager, processorState);
                Display(true, false, false);
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

        private void Display(bool displayProcessorSpecific, bool displayTowerSpecific, bool displayStrategySpecific)
        {
            foreach (GameObject go in processorSpecific)
            {
                go.SetActive(displayProcessorSpecific);
            }

            foreach (GameObject go in towerSpecific)
            {
                go.SetActive(displayTowerSpecific);
            }

            foreach (GameObject go in strategySpecific)
            {
                go.SetActive(displayStrategySpecific);
            }
        }

        private void Select(Description description)
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
                nameText.SetText(description.Name);
            }

            if (kills && description.Kills.HasValue)
            {
                kills.SetText(description.Kills.Value.ToString());
            }

            if (resell && description.Resell.HasValue)
            {
                resell.SetText(description.Resell.Value.ToString());
            }

            if (health && description.Health.HasValue)
            {
                health.Set(description.Health.Value);
            }

            if (charge)
            {
                charge.Set(description.Charge);
            }

            if (description.Priority.HasValue)
            {
                UpdatePriority(description.Priority.Value);
            }

            if (description.Strategy.HasValue)
            {
                UpdateStrategy(description.Strategy.Value, description.AvailableStrategies);
            }

            UpdateUpgrades(description.Upgrades, description.NextUpgrades, description.UpgradePathLocked);
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

        private void UpdatePriority(int priority)
        {
            if (priority >= 1 && priority <= nPriorities)
            {
                _toggles[priority - 1].SetIsOnWithoutNotify(true);
            }
            else if (priority < 1)
            {
                _toggles[0].SetIsOnWithoutNotify(true);
            }
            else
            {
                _toggles[^1].SetIsOnWithoutNotify(true);
            }
        }

        private void SetStrategy(int index)
        {
            TowerState selectedTower = GameManager.SelectedEntity.GetSelectedTower();
            if (selectedTower == null)
            {
                return;
            }

            GameManager.Tower.SetTargetStrategy(selectedTower, selectedTower.availableStrategies[index]);
        }

        private void UpdateStrategy(TargetStrategy strategy, TargetStrategy[] availableStrategies)
        {
            if (!availableStrategies.OrderBy(s => s).SequenceEqual(_lastStrategies.OrderBy(s => s)))
            {
                strategiesDropdown.options = availableStrategies.Select(s => new TMP_Dropdown.OptionData(s.GetDescription())).ToList();
                _lastStrategies = availableStrategies;
            }

            int index = Array.IndexOf(availableStrategies, strategy);
            if (index > 0)
            {
                strategiesDropdown.SetValueWithoutNotify(index);
            }
            else
            {
                strategiesDropdown.SetValueWithoutNotify(0);
            }
        }

        private void UpdateUpgrades(IReadOnlyList<UIUpgradeDescription[]> upgrades, IReadOnlyList<int> nextUpgrades, IReadOnlyList<bool> upgradePathLocked)
        {
            if (upgrades != null)
            {
                for (int i = _upgradePaths.Count; i < upgrades.Count; i++)
                {
                    UIUpgradePath upgradePath = Instantiate(upgradePathPrefab, upgradePathRoot);
                    _upgradePaths.Add(upgradePath);
                }

                for (int i = 0; i < _upgradePaths.Count; i++)
                {
                    _upgradePaths[i].gameObject.SetActive(i < upgrades.Count);

                    if (i < upgrades.Count)
                    {
                        _upgradePaths[i].SetUpgrades(
                            i,
                            upgrades[i],
                            nextUpgrades == null ? -1 : nextUpgrades[i],
                            upgradePathLocked != null && upgradePathLocked[i]
                        );
                    }
                }
            }
            else
            {
                foreach (UIUpgradePath t in _upgradePaths)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }

        private class Description
        {
            public string Name;
            public int? Kills;
            public int? Resell;
            public GaugeState? Health;
            public GaugeState Charge;
            public int? Priority;
            public TargetStrategy[] AvailableStrategies;
            public TargetStrategy? Strategy;
            public UIUpgradeDescription[][] Upgrades;
            public int[] NextUpgrades;
            public bool[] UpgradePathLocked;

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
                    Priority = towerState.priority,
                    AvailableStrategies = towerState.availableStrategies,
                    Strategy = towerState.targetStrategy,
                    Upgrades = new[]
                    {
                        towerState.config.upgradePath1.Select((u, i) => UIUpgradeDescription.From(u, towerState.id, 1, i)).ToArray(),
                        towerState.config.upgradePath2.Select((u, i) => UIUpgradeDescription.From(u, towerState.id, 2, i)).ToArray()
                    },
                    NextUpgrades = new[] { towerState.nextUpgradePath1, towerState.nextUpgradePath2 },
                    UpgradePathLocked = new[] { false, false },
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
