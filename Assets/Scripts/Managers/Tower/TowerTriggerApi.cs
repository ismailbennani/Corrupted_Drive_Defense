using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Enemies.Effects;
using GameEngine.Map;
using GameEngine.Shapes;
using GameEngine.Towers;
using Managers.Enemy;
using Managers.Map;
using UnityEngine;

namespace Managers.Tower
{
    public class TowerTriggerApi
    {
        public void Update(TowerState tower)
        {
            TriggerIfPossible(tower);
        }

        private TowerApi _towerApi;
        private readonly GameStateApi _gameStateApi;
        private readonly MapApi _mapApi;
        private readonly EnemyApi _enemyApi;

        public TowerTriggerApi(GameStateApi gameStateApi, MapApi mapApi, EnemyApi enemyApi)
        {
            _gameStateApi = gameStateApi;
            _mapApi = mapApi;
            _enemyApi = enemyApi;
        }

        public IEnumerable<EnemyState> GetTargets(TowerState tower)
        {
            switch (tower.description.targetType)
            {
                case TargetType.None:
                    return Enumerable.Empty<EnemyState>();
                case TargetType.AreaAtSelf:
                    return GetEnemiesInArea(tower.description.targetShape, tower.rotated, tower.cells.Select(c => c.gridPosition).ToArray());
                case TargetType.Single:
                case TargetType.AreaAtTarget:
                default:
                    break;
            }

            IEnumerable<EnemyState> enemies = _gameStateApi.GetEnemies();

            IEnumerable<EnemyState> orderedEnemies = tower.targetStrategy switch
            {
                TargetStrategy.First => enemies.OrderByDescending(t => t.pathIndex).ThenByDescending(t => t.pathCellCompletion),
                TargetStrategy.Last => enemies.OrderBy(t => t.pathIndex).ThenBy(t => t.pathCellCompletion),
                TargetStrategy.Close => enemies.OrderBy(t => _mapApi.ComputeDistance(_gameStateApi.GetEnemyCell(t), tower.cells))
                    .ThenBy(t => -t.pathCellCompletion),
                TargetStrategy.Strong => enemies.OrderByDescending(t => t.strength),
                _ => throw new ArgumentOutOfRangeException()
            };

            IEnumerable<Vector2Int> rangeCells = tower.description.range.EvaluateAt(tower.cells.Select(c => c.gridPosition).ToArray());
            IEnumerable<EnemyState> potentialTargets = _gameStateApi.FilterEnemiesInRange(orderedEnemies, rangeCells);

            if (!potentialTargets.Any())
            {
                return Enumerable.Empty<EnemyState>();
            }

            EnemyState target = potentialTargets.First();
            WorldCell targetCell = _gameStateApi.GetEnemyCell(target);
            
            if (tower.description.targetType == TargetType.Single)
            {
                int nTargets = tower.description.effect.ricochet + 1;
                return enemies.OrderBy(e => _mapApi.ComputeDistance(_gameStateApi.GetEnemyCell(e), targetCell)).Take(nTargets);
            }

            if (tower.description.targetType == TargetType.AreaAtTarget)
            {
                return GetEnemiesInArea(tower.description.targetShape, tower.rotated, targetCell.gridPosition);
            }

            throw new ArgumentOutOfRangeException(nameof(tower.description.targetType), tower.description.targetType, "invalid target type");
        }

        private IEnumerable<EnemyState> GetEnemiesInArea(IShape shape, bool rotated, params Vector2Int[] cells)
        {
            IEnumerable<Vector2Int> targetCells = shape.EvaluateAt(cells, rotated);
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

            if (tower.description.effect.damage > 0)
            {
                _enemyApi.Hit(targets.Select(t => t.id), tower.description.effect.damage, tower);
            }

            foreach (EnemyPassiveEffect effect in tower.description.effect.passiveEffects)
            {
                _gameStateApi.ApplyEnemyEffect(targets, effect, tower);
            }
        }
    }
}
