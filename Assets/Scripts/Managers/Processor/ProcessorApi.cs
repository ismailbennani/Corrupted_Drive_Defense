using GameEngine.Enemies;
using GameEngine.Processor;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Managers.Processor
{
    public class ProcessorApi
    {
        public readonly UnityEvent Lose = new();
        
        public readonly ProcessorUpdateApi Update;

        private readonly GameStateApi _gameStateApi;

        public ProcessorApi(GameStateApi gameStateApi)
        {
            Assert.IsNotNull(gameStateApi);

            _gameStateApi = gameStateApi;
            Update = new ProcessorUpdateApi(_gameStateApi);
        }

        public int Hit(EnemyState enemy)
        {
            // TODO: implement formula that depends on enemy
            int damage = 1;

            ProcessorState state = _gameStateApi.GetProcessorState();

            state.health.Consume(damage);

            if (state.health.Empty && Lose != null)
            {
                Lose.Invoke();
            }

            return damage;
        }
    }
}
