using System;
using GameEngine.Map;
using UnityEngine;

namespace GameEngine.State
{
    [Serializable]
    public struct TowerState
    {
        public Cell cell;
        
        [Header("Charge")]
        public int charge;
        public int maxCharge;
    }
}
