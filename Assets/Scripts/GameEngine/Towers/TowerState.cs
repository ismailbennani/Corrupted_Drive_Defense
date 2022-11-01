using System;
using GameEngine.Map;
using UnityEngine;
using Utils;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerState
    {
        public long id;
        public WorldCell cell;

        public GaugeState charge;
        public int kills;

        [Space(10)]
        public TowerConfig config;

        public TowerState(long id, WorldCell cell, TowerConfig config)
        {
            this.id = id;
            this.cell = cell;
            this.config = config;

            charge = new GaugeState(0, config.maxCharge);
        }
    }
}
