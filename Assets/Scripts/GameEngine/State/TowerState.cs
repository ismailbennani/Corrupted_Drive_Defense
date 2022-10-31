using System;
using GameComponents;
using GameEngine.Map;
using UnityEngine;

namespace GameEngine.State
{
    [Serializable]
    public struct TowerState
    {
        public long id;
        public WorldCell cell;

        [Header("Charge")]
        public GaugeState charge;
    }
}
