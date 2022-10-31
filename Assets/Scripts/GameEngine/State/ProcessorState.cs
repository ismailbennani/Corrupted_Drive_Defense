using System;
using UnityEngine;

namespace GameEngine.State
{
    [Serializable]
    public struct ProcessorState
    {
        public Vector2Int position;
        
        [Header("Health")]
        public int health;
        public int maxHealth;
        
        [Header("Ticks")]
        public int ticks;
        public int maxTicks;
    }
}
