using System;
using GameComponents;
using GameEngine.Map;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public struct TowerState
    {
        public long id;
        public WorldCell cell;

        [Header("Charge")]
        public GaugeState charge;

        [Space(10)]
        public TowerConfig config;
    }
}
