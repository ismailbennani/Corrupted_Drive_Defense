using System;
using System.Collections.Generic;
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

            int nextUpgradeInPath = state.GetNextUpgradeInPath(path);
            IReadOnlyList<TowerUpgrade> upgradePath = state.GetUpgradePath(path);

            if (nextUpgradeInPath < 0 || nextUpgradeInPath >= upgradePath.Count)
            {
                Debug.LogWarning($"{state.name}: cannot buy upgrade in path {path} because current upgrade is {nextUpgradeInPath}");
                return;
            }

            TowerUpgrade upgrade = upgradePath[nextUpgradeInPath];

            if (!CanBuy(state, upgrade))
            {
                Debug.LogWarning($"{state.name}: cannot buy {upgrade.upgradeName}");
                return;
            }

            _gameStateApi.Spend(upgrade.cost);

            state.BuyNextUpgrade(path);

            Debug.Log($"Bought upgrade {upgrade.upgradeName} (path {path}) of tower {state.name}");

            state.Refresh();
        }

        public bool CanBuy(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (tower.IsUpgradeLocked(towerUpgrade))
            {
                return false;
            }

            if (!tower.GetUpgradePathAndIndex(towerUpgrade, out int path, out int index))
            {
                return false;
            }

            int nextUpgrade = tower.GetNextUpgradeInPath(path);

            if (index != nextUpgrade)
            {
                return false;
            }

            return _gameStateApi.CanSpend(towerUpgrade.cost);
        }
    }
}
