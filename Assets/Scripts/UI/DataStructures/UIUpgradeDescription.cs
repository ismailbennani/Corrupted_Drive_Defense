using System;
using GameEngine.Processor;
using GameEngine.Towers;
using Managers;
using UnityEngine;

namespace UI.DataStructures
{
    [Serializable]
    public class UIUpgradeDescription
    {
        public string name;
        public int cost;
        public Sprite sprite;
        public string description;

        public bool bought;
        public bool canBeBought;
        public bool isLocked;

        public bool isTowerUpgrade;
        public long towerId;
        public int upgradePath;
        public int upgradeIndex;

        public static UIUpgradeDescription From(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (towerUpgrade == null)
            {
                return null;
            }

            GameManager.Instance.Tower.Upgrade.RequireUpgradePathAndIndex(tower, towerUpgrade, out int upgradePath, out int upgradeIndex);

            return new UIUpgradeDescription
            {
                name = towerUpgrade.upgradeName,
                cost = towerUpgrade.cost,
                sprite = towerUpgrade.sprite,
                description = towerUpgrade.upgradeName,

                bought = GameManager.Instance.Tower.Upgrade.IsUpgradeBought(tower, towerUpgrade),
                canBeBought = GameManager.Instance.Tower.Upgrade.CanUpgradeBeBought(tower, towerUpgrade),
                isLocked = GameManager.Instance.Tower.Upgrade.IsUpgradeLocked(tower, towerUpgrade),

                isTowerUpgrade = true,
                towerId = tower.id,
                upgradePath = upgradePath,
                upgradeIndex = upgradeIndex
            };
        }

        public static UIUpgradeDescription From(ProcessorState processorState, ProcessorUpgradeType processorUpgrade)
        {
            if (processorState == null)
            {
                return null;
            }

            ProcessorUpgradeConfig config = processorUpgrade switch
            {
                ProcessorUpgradeType.ChargeRate => processorState.config.chargeRateUpgrade,
                ProcessorUpgradeType.MaxCharge => processorState.config.maxChargeUpgrade,
                _ => throw new ArgumentOutOfRangeException(nameof(processorUpgrade), processorUpgrade, null)
            };

            int upgradePath = processorUpgrade switch
            {
                ProcessorUpgradeType.ChargeRate => 0,
                ProcessorUpgradeType.MaxCharge => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(processorUpgrade), processorUpgrade, null)
            };

            int alreadyBought = processorState.GetUpgrade(processorUpgrade);

            return new UIUpgradeDescription
            {
                name = config.upgradeName,
                cost = config.cost,
                sprite = config.sprite,
                description = $"+{config.upgrade} {processorUpgrade.GetDisplayName()}{Environment.NewLine}{alreadyBought}/{config.max}",

                bought = false,
                canBeBought = true,
                isLocked = false,

                isTowerUpgrade = false,
                upgradePath = upgradePath,
                upgradeIndex = 0
            };
        }
    }
}
