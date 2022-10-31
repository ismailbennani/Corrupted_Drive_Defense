using System;
using GameEngine.Map;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public class ProcessorState
    {
        public long id;
        public WorldCell cell;
        
        [Header("Health")]
        public int health;
        public int maxHealth;
        
        [Header("Ticks")]
        public int ticks;
        public int maxTicks;

        public ProcessorState(long id, WorldCell cell)
        {
            this.id = id;
            this.cell = cell;
        }
    }
}
