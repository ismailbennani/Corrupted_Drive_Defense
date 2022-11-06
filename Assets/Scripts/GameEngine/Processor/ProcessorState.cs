using System;
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

        public ProcessorConfig config;

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
            description = RefreshDescription();

            charge.SetMax(description.maxCharge);
        }
        
        public int GetUpgrade(ProcessorUpgradeType upgradeType)
        {
            int index = Array.IndexOf(availableUpgrades, upgradeType);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, "invalid upgrade type");
            }

            return upgrades[index];
        }

        public bool CanUpgrade(ProcessorUpgradeType upgradeType)
        {
            int max = upgradeType switch
            {
                ProcessorUpgradeType.ChargeRate => config.chargeRateUpgrade.max,
                ProcessorUpgradeType.MaxCharge => config.maxChargeUpgrade.max,
                _ => throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, null)
            };

            return GetUpgrade(upgradeType) < max;
        }

        public int UpgradeCost(ProcessorUpgradeType upgradeType)
        {
            return upgradeType switch
            {
                ProcessorUpgradeType.ChargeRate => config.chargeRateUpgrade.cost,
                ProcessorUpgradeType.MaxCharge => config.maxChargeUpgrade.cost,
                _ => throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, null)
            };
        }

        public int AddUpgrade(ProcessorUpgradeType upgradeType)
        {
            if (!CanUpgrade(upgradeType))
            {
                throw new InvalidOperationException($"Cannot upgrade {upgradeType}");
            }
            
            int index = Array.IndexOf(availableUpgrades, upgradeType);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, "invalid upgrade type");
            }
                
            return ++upgrades[index];
        }
        
        private ProcessorDescription RefreshDescription()
        {
            int chargeRateUpgrade = Mathf.Min(GetUpgrade(ProcessorUpgradeType.ChargeRate), config.chargeRateUpgrade.max);
            int maxChargeUpgrade = Mathf.Min(GetUpgrade(ProcessorUpgradeType.MaxCharge), config.maxChargeUpgrade.max);
            
            return new ProcessorDescription
            {
                chargeRate = config.chargeRate + chargeRateUpgrade * config.chargeRateUpgrade.upgrade,
                maxCharge = config.maxCharge + maxChargeUpgrade * config.maxChargeUpgrade.upgrade,
            };
        }
    }
}
