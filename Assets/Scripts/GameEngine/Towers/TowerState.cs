using System;
using System.Linq;
using GameEngine.Map;
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

        public GaugeState charge;
        public int kills;

        [Space(10)]
        public int totalCost;

        [Space(10)]
        public TowerConfig config;

        public TowerState(long id, Vector2Int cell, TowerConfig config)
        {
            this.id = id;
            cells = config.shape.EvaluateAt(cell).Select(GameManager.Instance.Map.GetCellAt).ToArray();
            this.config = config;

            charge = new GaugeState(0, config.maxCharge);

            totalCost = config.cost;
        }
    }
}
