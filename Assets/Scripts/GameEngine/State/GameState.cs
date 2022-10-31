using System;

namespace GameEngine.State
{
    [Serializable]
    public struct GameState
    {
        public ProcessorState processorState;
        public TowerState[] towerStates;
        public int currentWave;
    }
}
