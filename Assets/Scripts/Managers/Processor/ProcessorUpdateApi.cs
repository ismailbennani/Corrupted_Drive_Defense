using System;
using GameEngine.Processor;

namespace Managers.Processor
{
    public class ProcessorUpdateApi
    {
        private readonly GameStateApi _gameStateApi;

        public ProcessorUpdateApi(GameStateApi gameStateApi)
        {
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
    }
}
