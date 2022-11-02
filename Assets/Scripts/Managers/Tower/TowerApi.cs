using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Map;
using GameEngine.Shapes;
using GameEngine.Towers;
using Managers.Enemy;
using UnityEngine;

namespace Managers.Tower
{
    public class TowerApi
    {
        private readonly GameStateApi _gameStateApi;
        private readonly EnemyApi _enemyApi;

        public TowerApi(GameStateApi gameStateApi, EnemyApi enemyApi)
        {
            _gameStateApi = gameStateApi;
            _enemyApi = enemyApi;
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

        public void TriggerIfPossible(TowerState tower)
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

            if (tower.config.baseDamage > 0)
            {
                _enemyApi.Hit(targets.Select(t => t.id), tower.config.baseDamage, tower);
            }

            if (tower.config.towerHitEffect.enabled)
            {
                _gameStateApi.ApplyEnemyEffect(targets, tower.config.towerHitEffect.enemyEffect, tower);
            }
        }

        private IEnumerable<EnemyState> GetEnemiesInArea(IShape shape, params Vector2Int[] cells)
        {
            IEnumerable<Vector2Int> targetCells = shape.EvaluateAt(cells);
            return _gameStateApi.GetEnemiesAt(targetCells).ToArray();
        }
    }
}
