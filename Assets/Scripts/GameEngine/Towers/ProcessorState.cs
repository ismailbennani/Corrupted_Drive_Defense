using System;
using GameEngine.Map;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public struct ProcessorState
    {
        public long id;
        public WorldCell cell;
        
        [Header("Health")]
        public int health;
        public int maxHealth;
        
        [Header("Ticks")]
        public int ticks;
        public int maxTicks;
    }
}
