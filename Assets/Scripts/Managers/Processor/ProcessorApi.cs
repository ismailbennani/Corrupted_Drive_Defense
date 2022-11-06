using System;
using GameEngine.Enemies;
using GameEngine.Processor;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Managers.Processor
{
    public class ProcessorApi
    {
        public readonly UnityEvent Lose = new();

        private readonly GameStateApi _gameStateApi;

        public ProcessorApi(GameStateApi gameStateApi)
        {
            Assert.IsNotNull(gameStateApi);

            _gameStateApi = gameStateApi;
        }

        public void BuyUpgrade(int path)
        {
            ProcessorState state = _gameStateApi.GetProcessorState();
            BuyUpgrade(state.availableUpgrades[path]);
        }

        public void BuyUpgrade(ProcessorUpgradeType upgrade)
        {
            ProcessorState state = _gameStateApi.GetProcessorState();

            if (!state.CanUpgrade(upgrade))
            {
                throw new InvalidOperationException($"Cannot upgrade {upgrade}");
            }

            int cost = state.UpgradeCost(upgrade);

            if (!_gameStateApi.CanSpend(cost))
            {
                throw new InvalidOperationException($"Cannot upgrade {upgrade}: not enough money");
            }

            _gameStateApi.Spend(cost);

            state.AddUpgrade(upgrade);

            state.Refresh();
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
