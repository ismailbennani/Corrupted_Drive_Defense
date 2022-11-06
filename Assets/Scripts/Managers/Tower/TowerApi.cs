using System.Linq;
using GameEngine;
using GameEngine.Processor;
using GameEngine.Towers;
using Managers.Enemy;
using Managers.Map;
using UnityEngine;

namespace Managers.Tower
{
    public class TowerApi
    {
        public readonly TowerUpgradeApi Upgrade;
        public readonly TowerTriggerApi TowerTrigger;

        private readonly GameConfig _gameConfig;
        private readonly GameStateApi _gameStateApi;
        private readonly MapApi _mapApi;
        private readonly TowerSpawnerApi _towerSpawnerApi;
        private readonly SelectedEntityApi _selectedEntityApi;
        private readonly EnemyApi _enemyApi;

        public TowerApi(
            GameConfig gameConfig,
            GameStateApi gameStateApi,
            MapApi mapApi,
            TowerSpawnerApi towerSpawnerApi,
            SelectedEntityApi selectedEntityApi,
            EnemyApi enemyApi
        )
        {
            _gameConfig = gameConfig;
            _gameStateApi = gameStateApi;
            _mapApi = mapApi;
            _towerSpawnerApi = towerSpawnerApi;
            _selectedEntityApi = selectedEntityApi;
            _enemyApi = enemyApi;

            Upgrade = new TowerUpgradeApi(_gameStateApi);
            TowerTrigger = new TowerTriggerApi(_gameStateApi, _mapApi, _enemyApi);
        }

        public void Update()
        {
            foreach (TowerState tower in _gameStateApi.GetTowers().OrderByDescending(t => t.priority))
            {
                TowerTrigger.Update(tower);
                UpdateCharge(tower);
            }
        }

        public int SellValue(TowerState tower)
        {
            return Mathf.FloorToInt(_gameConfig.towerResellCoefficient * tower.totalCost);
        }

        public void Sell(TowerState tower)
        {
            int value = SellValue(tower);

            _gameStateApi.RemoveTower(tower.id);
            _towerSpawnerApi.DestroyTower(tower.id);
            _gameStateApi.Earn(value);

            _selectedEntityApi.Clear();
        }

        public void SetPriority(TowerState selectedTower, int priority)
        {
            selectedTower.priority = priority;
        }

        public void SetTargetStrategy(TowerState selectedTower, TargetStrategy targetStrategy)
        {
            selectedTower.targetStrategy = targetStrategy;
        }


        private void UpdateCharge(TowerState tower)
        {
            ProcessorState processorState = _gameStateApi.GetProcessorState();

            float requiredCharge = tower.charge.GetRemaining();
            float maxCharge = Time.deltaTime * tower.description.maxCharge / tower.description.fullChargeDelay;

            float consumed = processorState.charge.Consume(Mathf.Min(requiredCharge, maxCharge));

            if (tower.charge.Add(consumed) > 0)
            {
                tower.controller.SendMessage("SetCharge", tower.charge);
            }
        }
    }
}
