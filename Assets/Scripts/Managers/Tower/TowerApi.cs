﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Enemies;
using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Shapes;
using GameEngine.Towers;
using Managers.Enemy;
using Managers.Map;
using UnityEngine;

namespace Managers.Tower
{
    public class TowerApi
    {
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
        }

        public void Update()
        {
            foreach (TowerState tower in _gameStateApi.GetTowers().OrderByDescending(t => t.priority))
            {
                TriggerIfPossible(tower);
                UpdateCharge(tower);
            }
        }

        public void BuyUpgrade(long id, int path)
        {
            BuyUpgrade(_gameStateApi.GetTowerState(id), path);
        }

        public void BuyUpgrade(TowerState state, int path)
        {
            if (path != 0 && path != 1)
            {
                throw new InvalidOperationException($"Unknown path {path}");
            }

            int currentUpgradeInPath = path == 0 ? state.nextUpgradePath1 : state.nextUpgradePath2;
            TowerUpgrade[] upgradePath = path == 0 ? state.config.upgradePath1 : state.config.upgradePath2;

            if (currentUpgradeInPath < 0 || currentUpgradeInPath >= upgradePath.Length)
            {
                Debug.LogWarning($"{state.config.towerName}: cannot buy upgrade in path {path} because current upgrade is {currentUpgradeInPath}");
                return;
            }
            
            TowerUpgrade upgrade = upgradePath[currentUpgradeInPath];

            if (!CanUpgradeBeBought(state, upgrade))
            {
                Debug.LogWarning($"{state.config.towerName}: cannot buy {upgrade.upgradeName}");
                return;
            }

            _gameStateApi.Spend(upgrade.cost);

            if (path == 0)
            {
                state.nextUpgradePath1++;
            }
            else
            {
                state.nextUpgradePath2++;
            }

            Debug.Log($"Bought upgrade {upgrade.upgradeName} (path {path}) of tower {state.config.towerName}");

            state.Refresh();
        }

        public void RequireUpgradePathAndIndex(TowerState tower, TowerUpgrade towerUpgrade, out int upgradePath, out int upgradeIndex)
        {
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out upgradePath, out upgradeIndex))
            {
                throw new InvalidOperationException($"Invalid upgrade {towerUpgrade.upgradeName} for tower {tower.config.towerName}");
            }
        }

        public bool GetUpgradePathAndIndex(TowerState tower, TowerUpgrade towerUpgrade, out int upgradePath, out int upgradeIndex)
        {
            upgradePath = -1;
            upgradeIndex = -1;

            if (tower == null || !towerUpgrade)
            {
                return false;
            }

            int[] findUpgrade = tower.config.UpgradePaths.Select(path => Array.FindIndex(path, u => u.upgradeName == towerUpgrade.upgradeName)).ToArray();

            upgradePath = Array.FindIndex(findUpgrade, i => i >= 0);
            if (upgradePath < 0)
            {
                return false;
            }

            upgradeIndex = findUpgrade.First(i => i >= 0);

            return true;
        }

        public bool IsUpgradeBought(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out int path, out int index))
            {
                return false;
            }

            return path switch
            {
                0 => index < tower.nextUpgradePath1,
                1 => index < tower.nextUpgradePath2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public bool CanUpgradeBeBought(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (IsUpgradeLocked(tower, towerUpgrade))
            {
                return false;
            }
            
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out int path, out int index))
            {
                return false;
            }
            
            int nextUpgrade = path switch
            {
                0 => tower.nextUpgradePath1,
                1 => tower.nextUpgradePath2,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (index != nextUpgrade)
            {
                return false;
            }

            return _gameStateApi.CanSpend(towerUpgrade.cost);
        }

        public bool IsUpgradeLocked(TowerState tower, TowerUpgrade towerUpgrade)
        {
            if (!GetUpgradePathAndIndex(tower, towerUpgrade, out int path, out int index))
            {
                return true;
            }

            return false;
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

            IEnumerable<Vector2Int> rangeCells = tower.description.range.EvaluateAt(tower.cells.Select(c => c.gridPosition).ToArray());
            EnemyState[] potentialTargets = _gameStateApi.GetEnemiesAt(rangeCells).ToArray();

            if (!potentialTargets.Any())
            {
                return Enumerable.Empty<EnemyState>();
            }

            IEnumerable<EnemyState> orderedTargets;
            switch (tower.targetStrategy)
            {
                case TargetStrategy.First:
                    orderedTargets = potentialTargets.OrderByDescending(t => t.pathIndex).ThenBy(t => t.pathCellCompletion);
                    break;
                case TargetStrategy.Last:
                    orderedTargets = potentialTargets.OrderBy(t => t.pathIndex).ThenBy(t => t.pathCellCompletion);
                    break;
                case TargetStrategy.Close:
                    orderedTargets = potentialTargets.OrderBy(t => _mapApi.ComputeDistance(_gameStateApi.GetEnemyCell(t), tower.cells))
                        .ThenBy(t => -t.pathCellCompletion);
                    break;
                case TargetStrategy.Strong:
                    orderedTargets = potentialTargets.OrderByDescending(t => t.strength);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (tower.description.targetType == TargetType.Single)
            {
                return new[] { orderedTargets.First() };
            }

            WorldCell targetCell = _gameStateApi.GetEnemyCell(orderedTargets.First());

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

            if (tower.description.effect.applyPassiveEffect)
            {
                _gameStateApi.ApplyEnemyEffect(targets, tower.description.effect.passiveEffect, tower);
            }
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
