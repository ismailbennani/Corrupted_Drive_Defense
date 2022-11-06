using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using GameEngine.Map;
using GameEngine.Shapes;
using Managers;
using UnityEngine;
using Utils;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerState
    {
        public long id;
        public WorldCell[] cells;
        public bool rotated;
        public int priority;

        [Space(10)]
        public int nextUpgradePath1;
        public int nextUpgradePath2;
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
        public TowerConfig config;

        public TowerController controller;

        public TowerState(long id, Vector2Int cell, bool rotated, TowerConfig config)
        {
            this.id = id;
            cells = config.shape.EvaluateAt(cell, rotated).Select(GameManager.Instance.Map.GetCellAt).ToArray();
            this.rotated = config.canRotate && rotated;
            this.config = config;

            Refresh();
        }

        public void Refresh()
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
            IEnumerable<TowerUpgrade> upgrades1 = config.upgradePath1.Take(nextUpgradePath1);
            IEnumerable<TowerUpgrade> upgrades2 = config.upgradePath1.Take(nextUpgradePath2);
            TowerUpgrade[] upgrades = upgrades1.Concat(upgrades2).ToArray();

            if (upgrades.Any())
            {
                aggregatedUpgrade = upgrades.Aggregate(TowerUpgrade.CombineInPlace);
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
    }
}
