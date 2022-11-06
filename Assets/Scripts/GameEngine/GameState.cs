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
        public int money;

        public ProcessorState processorState;
        public List<TowerState> towerStates = new();
        public List<EnemyState> enemyStates = new();
        public int currentWave;

        public GameState(GameConfig gameConfig = null)
        {
            if (gameConfig != null)
            {
                money = gameConfig.startingMoney;
            }
        }

        public GameState Clone()
        {
            return new GameState
            {
                money = money,
                processorState = processorState,
                towerStates = towerStates.ToList(),
                enemyStates = enemyStates.ToList(),
                currentWave = currentWave
            };
        }
    }
}
