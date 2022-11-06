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

            tower.RequireUpgradePathAndIndex(towerUpgrade, out int upgradePath, out int upgradeIndex);

            return new UIUpgradeDescription
            {
                name = towerUpgrade.upgradeName,
                cost = towerUpgrade.cost,
                sprite = towerUpgrade.sprite,
                description = $"{towerUpgrade.description}{Environment.NewLine}{Environment.NewLine}{towerUpgrade.GetTechnicalDescription()}",

                bought = tower.IsUpgradeBought(towerUpgrade),
                canBeBought = GameManager.Instance.Tower.Upgrade.CanBuy(tower, towerUpgrade),
                isLocked = tower.IsUpgradeLocked(towerUpgrade),

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

            ProcessorUpgradeConfig config = processorState.GetUpgradeConfig(processorUpgrade);
            int path = processorState.GetUpgradePath(processorUpgrade);
            int alreadyBought = processorState.GetUpgradeLevel(processorUpgrade);

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
                upgradePath = path,
                upgradeIndex = 0
            };
        }
    }
}
