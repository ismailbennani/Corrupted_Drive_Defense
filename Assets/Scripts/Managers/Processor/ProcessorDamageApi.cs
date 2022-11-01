using GameEngine.Enemies;
using GameEngine.Processor;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Managers.Processor
{
    public class ProcessorDamageApi
    {
        public readonly UnityEvent Lose = new();
        
        private readonly GameStateApi _gameStateApi;

        public ProcessorDamageApi(GameStateApi gameStateApi)
        {
            Assert.IsNotNull(gameStateApi);
            
            _gameStateApi = gameStateApi;
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
