using System;
using GameEngine.Towers;
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

        public bool isTowerUpgrade;
        public long towerId;
        public int upgradePath;
        public int upgradeIndex;

        public static UIUpgradeDescription From(TowerUpgrade towerUpgrade, long towerId, int path, int index)
        {
            if (towerUpgrade == null)
            {
                return null;
            }

            return new UIUpgradeDescription
            {
                name = towerUpgrade.upgradeName,
                cost = towerUpgrade.cost,
                sprite = towerUpgrade.sprite,
                description = towerUpgrade.upgradeName,
                
                isTowerUpgrade = true,
                towerId = towerId,
                upgradePath = path,
                upgradeIndex = index,
            };
        }
    }
}
