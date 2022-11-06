using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using GameEngine.Shapes;
using Managers;
using UnityEngine;
using Utils;

namespace GameEngine.Processor
{
    [Serializable]
    public class ProcessorState
    {
        public WorldCell[] cells;

        public GaugeState health;
        public GaugeState charge;

        public ProcessorUpgradeType[] availableUpgrades;
        public int[] upgrades;
        public ProcessorDescription description;

        [SerializeField]
        private ProcessorConfig config;

        private Dictionary<ProcessorUpgradeType, int> _upgradeLevel;
        private Dictionary<ProcessorUpgradeType, ProcessorUpgradeConfig> _upgradePaths;

        public ProcessorState(Vector2Int cell, ProcessorConfig config)
        {
            cells = Shape.CellsInSquare(cell, config.size, config.offset).Select(GameManager.Instance.Map.GetCellAt).ToArray();
            this.config = config;

            health.SetMax(config.maxHealth);
            health.Set(config.maxHealth);

            availableUpgrades = Enum.GetValues(typeof(ProcessorUpgradeType)).OfType<ProcessorUpgradeType>().ToArray();
            upgrades = availableUpgrades.Select(_ => 0).ToArray();

            Refresh();
        }

        public void Refresh()
        {
            _upgradePaths = new Dictionary<ProcessorUpgradeType, ProcessorUpgradeConfig>
            {
                { ProcessorUpgradeType.ChargeRate, config.chargeRateUpgrade },
                { ProcessorUpgradeType.MaxCharge, config.maxChargeUpgrade },
            };

            _upgradeLevel = new Dictionary<ProcessorUpgradeType, int>
            {
                { ProcessorUpgradeType.ChargeRate, upgrades[Array.IndexOf(availableUpgrades, ProcessorUpgradeType.ChargeRate)] },
                { ProcessorUpgradeType.MaxCharge, upgrades[Array.IndexOf(availableUpgrades, ProcessorUpgradeType.MaxCharge)] },
            };
            
            description = RefreshDescription();

            charge.SetMax(description.maxCharge);
        }

        public int GetUpgradeLevel(ProcessorUpgradeType upgradeType)
        {
            return _upgradeLevel?[upgradeType] ?? 0;
        }

        public bool CanUpgrade(ProcessorUpgradeType upgradeType)
        {
            return GetUpgradeLevel(upgradeType) < _upgradePaths[upgradeType].max;
        }

        public int UpgradeCost(ProcessorUpgradeType upgradeType)
        {
            return _upgradePaths?[upgradeType].cost ?? -1;
        }

        public int AddUpgrade(ProcessorUpgradeType upgradeType)
        {
            if (!CanUpgrade(upgradeType))
            {
                throw new InvalidOperationException($"Cannot upgrade {upgradeType}");
            }

            int index = GetUpgradePath(upgradeType);
            return ++upgrades[index];
        }

        public ProcessorUpgradeConfig GetUpgradeConfig(ProcessorUpgradeType upgradeType)
        {
            return _upgradePaths?[upgradeType];
        }

        public int GetUpgradePath(ProcessorUpgradeType upgradeType)
        {
            int index = Array.IndexOf(availableUpgrades, upgradeType);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, "invalid upgrade type");
            }

            return index;
        }

        private ProcessorDescription RefreshDescription()
        {
            int chargeRateUpgrade = Mathf.Min(GetUpgradeLevel(ProcessorUpgradeType.ChargeRate), _upgradePaths[ProcessorUpgradeType.ChargeRate].max);
            int maxChargeUpgrade = Mathf.Min(GetUpgradeLevel(ProcessorUpgradeType.MaxCharge), _upgradePaths[ProcessorUpgradeType.MaxCharge].max);

            return new ProcessorDescription
            {
                chargeRate = config.chargeRate + chargeRateUpgrade * config.chargeRateUpgrade.upgrade,
                maxCharge = config.maxCharge + maxChargeUpgrade * config.maxChargeUpgrade.upgrade
            };
        }
    }
}
