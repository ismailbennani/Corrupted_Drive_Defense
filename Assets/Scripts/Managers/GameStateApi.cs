using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Enemies;
using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Towers;
using GameEngine.Waves;
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
            return _state.towerStates.SingleOrDefault(t => t.cell.gridPosition == cell);
        }

        public void AddTower(TowerState tower)
        {
            _state.towerStates.Add(tower);
        }

        public void AddKills(TowerState state, int nKills)
        {
            state.kills += nKills;
        }

        #endregion

        #region Enemy

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
                Debug.LogWarning($"Cannot find enemy state with id {id}");
                return;
            }

            _state.enemyStates.Remove(enemyState);
        }

        public IEnumerable<EnemyState> GetEnemiesAt(IEnumerable<Vector2Int> targetCells)
        {
            WorldCell[] path = _map.GetPath().ToArray();
            int[] pathCells = targetCells.Select(c => Array.FindIndex(path, w => w.gridPosition == c)).Where(i => i >= 0).ToArray();
            return _state.enemyStates.Where(e => pathCells.Contains(e.pathIndex)).ToArray();
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
