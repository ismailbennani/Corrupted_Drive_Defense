using System;
using System.Linq;
using GameEngine.Map;
using GameEngine.Shapes;
using Managers;
using UnityEngine;
using Utils;

namespace GameEngine.Processor
{
    [Serializable]
    public class ProcessorState
    {
        public WorldCell[] cells;
        
        public GaugeState health;
        public GaugeState charge;

        public ProcessorConfig config;
        
        public ProcessorState(Vector2Int cell, ProcessorConfig config)
        {
            cells = Shape.CellsInSquare(cell, config.size, config.offset).Select(GameManager.Instance.Map.GetCellAt).ToArray(); 
            this.config = config;

            health = new GaugeState(config.maxHealth, config.maxHealth);
            charge = new GaugeState(0, config.maxCharge);
        }
    }
}
