using System;
using GameComponents;
using GameEngine.Map;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerState
    {
        public long id;
        public WorldCell cell;

        [Header("Charge")]
        public GaugeState charge;

        [Space(10)]
        public TowerConfig config;

        public TowerState(long id, WorldCell cell, TowerConfig config)
        {
            this.id = id;
            this.cell = cell;
            this.config = config;
        }
    }
}
