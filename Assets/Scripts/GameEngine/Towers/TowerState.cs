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

            availableStrategies = config.targetType switch
            {
                TargetType.AreaAtSelf => Array.Empty<TargetStrategy>(),
                TargetType.Single => Enum.GetValues(typeof(TargetStrategy)).OfType<TargetStrategy>().ToArray(),
                TargetType.AreaAtTarget => Enum.GetValues(typeof(TargetStrategy)).OfType<TargetStrategy>().ToArray(),
                _ => throw new ArgumentOutOfRangeException()
            };

            charge = new GaugeState(0, config.maxCharge);

            totalCost = config.cost;
        }
    }
}
