using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Enemies;
using GameEngine.Enemies.Effects;
using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Towers;
using Managers.Map;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers
{
    public class GameStateApi
    {
        private readonly GameState _state;

        private readonly MapApi _map;

        public GameStateApi(GameState state, MapApi map)
        {
            Assert.IsNotNull(state);
            Assert.IsNotNull(map);

            _state = state;
            _map = map;
        }

        #region Money

        public int GetMoney()
        {
            return _state.money;
        }

        public void Earn(int money)
        {
            _state.money += money;
        }

        public bool CanSpend(int money)
        {
            return _state.money >= money;
        }

        public void Spend(int money)
        {
            if (!CanSpend(money))
            {
                throw new InvalidOperationException($"Not enough money: {money} > {_state.money}");
            }
            
            _state.money -= money;
        }

        #endregion

        #region Processor

        public ProcessorState GetProcessorState()
        {
            return _state.processorState;
        }

        public void SetProcessorState(ProcessorState state)
        {
            _state.processorState = state;
        }

        public bool IsProcessorCell(WorldCell cell)
        {
            return IsProcessorCell(cell.gridPosition);
        }

        public bool IsProcessorCell(Vector2Int cell)
        {
            return _state.processorState.cells.Any(c => c.gridPosition == cell);
        }

        #endregion

        #region Tower

        public IEnumerable<TowerState> GetTowers()
        {
            return _state.towerStates;
        }

        public TowerState GetTowerState(long id)
        {
            return _state.towerStates.SingleOrDefault(t => t.id == id);
        }

        public TowerState GetTowerAt(WorldCell cell)
        {
            return GetTowerAt(cell.gridPosition);
        }

        public TowerState GetTowerAt(Vector2Int cell)
        {
            return _state.towerStates.SingleOrDefault(t => t.cells.Any(c => c.gridPosition == cell));
        }

        public void AddTower(TowerState tower)
        {
            _state.towerStates.Add(tower);
        }

        public void RemoveTower(long id)
        {
            TowerState towerState = GetTowerState(id);
            if (towerState == null)
            {
                throw new InvalidOperationException($"Cannot find tower state with id {id}");
            }

            _state.towerStates.Remove(towerState);
        }

        public void AddKills(TowerState state, int nKills)
        {
            state.kills += nKills;
        }

        #endregion

        #region Enemy

        public IEnumerable<EnemyState> GetEnemies()
        {
            return _state.enemyStates;
        }

        public EnemyState GetEnemyState(long id)
        {
            return _state.enemyStates.SingleOrDefault(t => t.id == id);
        }

        public void AddEnemy(EnemyState enemy)
        {
            _state.enemyStates.Add(enemy);
        }

        public void RemoveEnemy(long id)
        {
            EnemyState enemyState = GetEnemyState(id);
            if (enemyState == null)
            {
                throw new InvalidOperationException($"Cannot find enemy state with id {id}");
            }

            _state.enemyStates.Remove(enemyState);
        }

        public WorldCell GetEnemyCell(EnemyState enemy)
        {
            WorldCell[] path = _map.GetPath().ToArray();
            if (enemy.pathIndex < 0)
            {
                return path[0];
            }

            if (enemy.pathIndex >= path.Length)
            {
                return path[^1];
            }

            return path[enemy.pathIndex];
        }

        public IEnumerable<EnemyState> GetEnemiesAt(IEnumerable<Vector2Int> targetCells)
        {
            WorldCell[] path = _map.GetPath().ToArray();
            int[] pathCells = targetCells.Select(c => Array.FindIndex(path, w => w.gridPosition == c)).Where(i => i >= 0).ToArray();
            return _state.enemyStates.Where(e => pathCells.Contains(e.pathIndex)).ToArray();
        }

        public void ApplyEnemyEffect(IEnumerable<EnemyState> enemies, EnemyEffect effect, TowerState source)
        {
            foreach (EnemyState enemy in enemies)
            {
                enemy.AddEffect(effect, source);
            }
        }

        #endregion

        #region Wave

        public int GetCurrentWave()
        {
            return _state.currentWave;
        }

        public void IncrementWave()
        {
            _state.currentWave++;
        }

        #endregion
    }
}
