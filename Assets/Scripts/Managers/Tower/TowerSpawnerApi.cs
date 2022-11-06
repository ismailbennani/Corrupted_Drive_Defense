using System.Linq;
using GameEngine.Map;
using GameEngine.Shapes;
using GameEngine.Towers;
using Managers.Map;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class TowerSpawnerApi
    {
        private readonly TowerSpawnerManager _towerSpawnerManager;
        private readonly GameStateApi _gameState;
        private readonly MapApi _map;

        public TowerSpawnerApi(TowerSpawnerManager towerSpawnerManager, GameStateApi gameState, MapApi map)
        {
            Assert.IsNotNull(towerSpawnerManager);
            Assert.IsNotNull(gameState);
            Assert.IsNotNull(map);

            _towerSpawnerManager = towerSpawnerManager;
            _gameState = gameState;
            _map = map;
        }

        public bool TrySpawnTower(TowerConfig tower, Vector2Int cell, bool rotated, out TowerState state)
        {
            if (!CanSpawnTowerAt(tower, cell, rotated, out string reason))
            {
                Debug.LogWarning($"Cannot spawn tower {tower.towerName}: {reason}");

                state = null;
                return false;
            }

            _gameState.Spend(tower.cost);

            WorldCell targetCell = _map.GetCellAt(cell);
            state = _towerSpawnerManager.SpawnTower(tower, targetCell, rotated);

            _gameState.AddTower(state);

            return true;
        }

        public bool CanSpawnTowerAt(TowerConfig tower, Vector2Int cell, bool rotated)
        {
            return CanSpawnTowerAt(tower, cell, rotated, out _);
        }

        public bool CanSpawnTowerAt(TowerConfig tower, Vector2Int cell, bool rotated, out string reason)
        {
            WorldCell[] worldCells = tower.shape.EvaluateAt(cell, rotated).Select(_map.GetCellAt).ToArray();

            if (worldCells.Any(c => c.type != CellType.Free))
            {
                reason = "tower overlaps path";
                return false;
            }

            if (worldCells.Intersect(_gameState.GetProcessorState().cells).Any())
            {
                reason = "tower overlaps processor";
                return false;
            }

            TowerState[] otherTowers = worldCells.Select(c => _gameState.GetTowerAt(c.gridPosition)).Where(t => t != null).ToArray();
            if (otherTowers.Any())
            {
                reason = $"tower overlaps other towers: {string.Join(", ", otherTowers.Select(t => $"{t.config.towerName} ({t.id})"))}";
                return false;
            }

            reason = null;
            return true;
        }

        public void DestroyTower(long id)
        {
            _towerSpawnerManager.DestroyTower(id);
        }
    }
}
