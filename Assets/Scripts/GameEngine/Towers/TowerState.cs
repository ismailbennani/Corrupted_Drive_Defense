using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using GameEngine.Map;
using GameEngine.Shapes;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerState
    {
        public UnityEvent<int> onUpgrade = new();
        
        public long id;
        public string name;
        public WorldCell[] cells;
        public bool rotated;
        public int priority;

        [Space(10)]
        [SerializeField]
        private int[] nextUpgradeInPath;
        public TowerUpgrade aggregatedUpgrade;
        public TowerDescription description;

        [Space(10)]
        public TargetStrategy targetStrategy;

        public TargetStrategy[] availableStrategies;

        [Space(10)]
        public GaugeState charge;

        public int kills;

        [Space(10)]
        public int totalCost;

        [Space(10)]
        [SerializeField]
        private TowerConfig config;

        public TowerController controller;

        public TowerState(long id, Vector2Int cell, bool rotated, TowerConfig config)
        {
            this.id = id;
            name = config.towerName;
            cells = config.shape.EvaluateAt(cell, rotated).Select(GameManager.Instance.Map.GetCellAt).ToArray();
            this.rotated = config.canRotate && rotated;
            this.config = config;

            nextUpgradeInPath = config.UpgradePaths.Select(u => 0).ToArray();

            Refresh();
        }

        public int GetNextUpgradeInPath(int path)
        {
            return nextUpgradeInPath[path];
        }

        public IReadOnlyList<TowerUpgrade> GetUpgradePath(int path)
        {
            return config.UpgradePaths[path];
        }

        public IReadOnlyList<IReadOnlyList<TowerUpgrade>> GetUpgradePaths()
        {
            return config.UpgradePaths.Cast<IReadOnlyList<TowerUpgrade>>().ToArray();
        }

        public int BuyNextUpgrade(int path)
        {
            nextUpgradeInPath[path]++;
            
            Refresh();
            
            onUpgrade?.Invoke(path);

            return nextUpgradeInPath[path];
        }

        public bool IsUpgradeBought(TowerUpgrade towerUpgrade)
        {
            if (!GetUpgradePathAndIndex(towerUpgrade, out int path, out int index))
            {
                return false;
            }

            return index < GetNextUpgradeInPath(path);
        }

        public bool IsUpgradeLocked(TowerUpgrade towerUpgrade)
        {
            if (!GetUpgradePathAndIndex(towerUpgrade, out int path, out int index))
            {
                return true;
            }

            int mainPath = GetMainUpgradePath();
            if (mainPath >= 0)
            {
                return path != mainPath && index > 0;
            }
            
            return false;
        }

        public void RequireUpgradePathAndIndex(TowerUpgrade towerUpgrade, out int upgradePath, out int upgradeIndex)
        {
            if (!GetUpgradePathAndIndex(towerUpgrade, out upgradePath, out upgradeIndex))
            {
                throw new InvalidOperationException($"Invalid upgrade {towerUpgrade.upgradeName} for tower {name}");
            }
        }

        public bool GetUpgradePathAndIndex(TowerUpgrade towerUpgrade, out int upgradePath, out int upgradeIndex)
        {
            upgradePath = -1;
            upgradeIndex = -1;

            if (!towerUpgrade)
            {
                return false;
            }

            int[] findUpgrade = config.UpgradePaths.Select(path => Array.FindIndex(path, u => u.upgradeName == towerUpgrade.upgradeName)).ToArray();

            upgradePath = Array.FindIndex(findUpgrade, i => i >= 0);
            if (upgradePath < 0)
            {
                return false;
            }

            upgradeIndex = findUpgrade.First(i => i >= 0);

            return true;
        }

        private void Refresh()
        {
            RecomputeDescription();
            RecomputeAvailableStrategies();

            charge.SetMax(description.maxCharge);

            totalCost = config.cost;
            if (aggregatedUpgrade)
            {
                totalCost += aggregatedUpgrade.cost;
            }
        }

        private void RecomputeDescription()
        {
            TowerUpgrade[] upgrades = config.UpgradePaths.SelectMany((p, i) => p.Take(nextUpgradeInPath[i])).ToArray();

            if (upgrades.Any())
            {
                TowerUpgrade emptyUpgrade = TowerUpgrade.GetEmpty();

                aggregatedUpgrade = upgrades.Aggregate(emptyUpgrade, TowerUpgrade.CombineInPlace);
                description = TowerDescription.Apply((TowerDescription)config.naked.Clone(), aggregatedUpgrade);
            }
            else
            {
                aggregatedUpgrade = null;
                description = config.naked;
            }
        }

        private void RecomputeAvailableStrategies()
        {
            availableStrategies = description.targetType switch
            {
                TargetType.None => Array.Empty<TargetStrategy>(),
                TargetType.AreaAtSelf => Array.Empty<TargetStrategy>(),
                TargetType.Single => Enum.GetValues(typeof(TargetStrategy)).OfType<TargetStrategy>().ToArray(),
                TargetType.AreaAtTarget => Enum.GetValues(typeof(TargetStrategy)).OfType<TargetStrategy>().ToArray(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private int GetMainUpgradePath()
        {
            return Array.FindIndex(nextUpgradeInPath, u => u > 1);
        }
    }
}
