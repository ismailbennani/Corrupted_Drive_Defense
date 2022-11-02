using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Enemies;
using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Shapes;
using GameEngine.Towers;
using Managers.Enemy;
using UnityEngine;

namespace Managers.Tower
{
    public class TowerApi
    {
        private readonly GameConfig _gameConfig;
        private readonly GameStateApi _gameStateApi;
        private readonly TowerSpawnerApi _towerSpawnerApi;
        private readonly SelectedEntityApi _selectedEntityApi;
        private readonly EnemyApi _enemyApi;

        public TowerApi(
            GameConfig gameConfig,
            GameStateApi gameStateApi,
            TowerSpawnerApi towerSpawnerApi,
            SelectedEntityApi selectedEntityApi,
            EnemyApi enemyApi
        )
        {
            _gameConfig = gameConfig;
            _gameStateApi = gameStateApi;
            _towerSpawnerApi = towerSpawnerApi;
            _selectedEntityApi = selectedEntityApi;
            _enemyApi = enemyApi;
        }

        public void Update()
        {
            foreach (TowerState tower in _gameStateApi.GetTowers())
            {
                TriggerIfPossible(tower);
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

        public IEnumerable<EnemyState> GetTargets(TowerState tower)
        {
            if (tower.config.targetType == TargetType.AreaAtSelf)
            {
                return GetEnemiesInArea(tower.config.targetShape, tower.cells.Select(c => c.gridPosition).ToArray());
            }

            IEnumerable<Vector2Int> rangeCells = tower.config.range.EvaluateAt(tower.cells.Select(c => c.gridPosition).ToArray());
            EnemyState[] potentialTargets = _gameStateApi.GetEnemiesAt(rangeCells).ToArray();

            if (!potentialTargets.Any())
            {
                return Enumerable.Empty<EnemyState>();
            }

            // TODO: select target based on some strategy
            EnemyState target = potentialTargets.First();

            if (tower.config.targetType == TargetType.Single)
            {
                return new[] { target };
            }

            WorldCell targetCell = _gameStateApi.GetEnemyCell(target);

            if (tower.config.targetType == TargetType.AreaAtTarget)
            {
                return GetEnemiesInArea(tower.config.targetShape, targetCell.gridPosition);
            }

            throw new ArgumentOutOfRangeException(nameof(tower.config.targetType), tower.config.targetType, "invalid target type");
        }

        private IEnumerable<EnemyState> GetEnemiesInArea(IShape shape, params Vector2Int[] cells)
        {
            IEnumerable<Vector2Int> targetCells = shape.EvaluateAt(cells);
            return _gameStateApi.GetEnemiesAt(targetCells).ToArray();
        }

        private void TriggerIfPossible(TowerState tower)
        {
            if (!tower.charge.Full)
            {
                return;
            }

            EnemyState[] targets = GetTargets(tower).ToArray();
            if (targets.Length == 0)
            {
                return;
            }

            tower.charge.Clear();
            tower.controller.SendMessage("SetCharge", tower.charge);

            if (tower.config.baseDamage > 0)
            {
                _enemyApi.Hit(targets.Select(t => t.id), tower.config.baseDamage, tower);
            }

            if (tower.config.towerHitEffect.enabled)
            {
                _gameStateApi.ApplyEnemyEffect(targets, tower.config.towerHitEffect.enemyEffect, tower);
            }
        }

        private void UpdateCharge(TowerState tower)
        {
            ProcessorState processorState = _gameStateApi.GetProcessorState();
            
            float requiredCharge = tower.charge.GetRemaining();
            float maxCharge = Time.deltaTime * tower.config.frequency;

            float consumed = processorState.charge.Consume(Mathf.Min(requiredCharge, maxCharge));

            if (tower.charge.Add(consumed) > 0)
            {
                tower.controller.SendMessage("SetCharge", tower.charge);
            }
        }
    }
}
