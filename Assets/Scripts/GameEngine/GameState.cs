using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Processor;
using GameEngine.Towers;

namespace GameEngine
{
    [Serializable]
    public class GameState
    {
        public ProcessorState processorState;
        public List<TowerState> towerStates = new();
        public List<EnemyState> enemyStates = new();
        public int currentWave;

        public GameState Clone()
        {
            return new GameState
            {
                processorState = processorState,
                towerStates = towerStates.ToList(),
                currentWave = currentWave,
            };
        }
    }
}
