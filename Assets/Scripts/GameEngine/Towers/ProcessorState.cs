using System;
using GameEngine.Map;
using Utils;

namespace GameEngine.Towers
{
    [Serializable]
    public class ProcessorState
    {
        public WorldCell cell;
        
        public GaugeState health;
        public GaugeState charge;

        public ProcessorConfig config;
        
        public ProcessorState(WorldCell cell, ProcessorConfig config)
        {
            this.cell = cell;
            this.config = config;

            health = new GaugeState(config.maxHealth, config.maxHealth);
            charge = new GaugeState(0, config.maxCharge);
        }
    }
}
