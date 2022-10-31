using System;
using UnityEngine;

namespace GameEngine.State
{
    [Serializable]
    public struct TowerState
    {
        public Vector2Int position;
        
        [Header("Charge")]
        public int charge;
        public int maxCharge;
    }
}
