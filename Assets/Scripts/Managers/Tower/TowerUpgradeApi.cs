using System;
using System.Linq;
using GameEngine.Towers;
using UnityEngine;

namespace Managers.Tower
{
    public class TowerUpgradeApi
    {
        private readonly GameStateApi _gameStateApi;

        public TowerUpgradeApi(GameStateApi gameStateApi)
        {
            _gameStateApi = gameStateApi;

        }

        public void BuyUpgrade(long id, int path)
        {
            BuyUpgrade(_gameStateApi.GetTowerState(id), path);
        }

        public void BuyUpgrade(TowerState state, int path)
        {
            if (path != 0 && path != 1)
            {
                throw new InvalidOperationException($"Unknown path {path}");
            }

            int currentUpgradeInPath = path == 0 ? state.nextUpgradePath1 : state.nextUpgradePath2;
            TowerUpgrade[] upgradePath = path == 0 ? state.config.upgradePath1 : state.config.upgradePath2;

            if (currentUpgradeInPath < 0 || currentUpgradeInPath >= upgradePath.Length)
            {
                Debug.LogWarning($"{state.config.towerName}: cannot buy upgrade in path {path} because current upgrade is {currentUpgradeInPath}");
                return;
            }
            
            TowerUpgrade upgrade = upgradePath[currentUpgradeInPath];

            if (!CanUpgradeBeBought(state, upgrade))
            {
                Debug.LogWarning($"{state.config.towerName}: cannot buy {upgrade.upgradeName}");
                return;
            }

            _gameStateApi.Spend(upgrade.cost);

            if (path == 0)
            {
                state.nextUpgradePath1++;
            }
            else
            {
                state.nextUpgradePath2++;
            }

            Debug.Log($"Bought upgrade {upgrade.upgradeName} (path {path}) of tower {state.config.towerName}");

            state.Refresh();
        }

        public void RequireUpgradePathAndIndex(TowerState tower, TowerUpgrade towerUpgrade, out int upgradePath, out int upgradeIndex)
        {
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out upgradePath, out upgradeIndex))
            {
                throw new InvalidOperationException($"Invalid upgrade {towerUpgrade.upgradeName} for tower {tower.config.towerName}");
            }
        }

        public bool GetUpgradePathAndIndex(TowerState tower, TowerUpgrade towerUpgrade, out int upgradePath, out int upgradeIndex)
        {
            upgradePath = -1;
            upgradeIndex = -1;

            if (tower == null || !towerUpgrade)
            {
                return false;
            }

            int[] findUpgrade = tower.config.UpgradePaths.Select(path => Array.FindIndex<TowerUpgrade>(path, u => u.upgradeName == towerUpgrade.upgradeName)).ToArray();

            upgradePath = Array.FindIndex(findUpgrade, i => i >= 0);
            if (upgradePath < 0)
            {
                return false;
            }

            upgradeIndex = findUpgrade.First(i => i >= 0);

            return true;
        }

        public bool IsUpgradeBought(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out int path, out int index))
            {
                return false;
            }

            return path switch
            {
                0 => index < tower.nextUpgradePath1,
                1 => index < tower.nextUpgradePath2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public bool CanUpgradeBeBought(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (IsUpgradeLocked(tower, towerUpgrade))
            {
                return false;
            }
            
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out int path, out int index))
            {
                return false;
            }
            
            int nextUpgrade = path switch
            {
                0 => tower.nextUpgradePath1,
                1 => tower.nextUpgradePath2,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (index != nextUpgrade)
            {
                return false;
            }

            return _gameStateApi.CanSpend(towerUpgrade.cost);
        }

        public bool IsUpgradeLocked(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out int path, out int index))
            {
                return true;
            }

            return false;
        }
    }
}
