using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
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

        public bool TrySpawnTower(TowerConfig tower, Vector2Int cell, out TowerState state, bool force = false, bool register = true)
        {
            if (!force && !CanSpawnTower(tower, cell, out string reason))
            {
                Debug.LogWarning($"Cannot spawn tower {tower.towerName}: {reason}");

                state = null;
                return false;
            }

            WorldCell targetCell = _map.GetCellAt(cell);
            _towerSpawnerManager.SpawnTower(tower, targetCell, out state);

            if (register)
            {
                _gameState.AddTower(state);
            }

            return true;
        }

        public bool CanSpawnTower(TowerConfig tower, Vector2Int cell)
        {
            return CanSpawnTower(tower, cell, out _);
        }

        public bool CanSpawnTower(TowerConfig tower, Vector2Int cell, out string reason)
        {
            WorldCell[] worldCells = tower.shape.EvaluateAt(cell).Select(_map.GetCellAt).ToArray();

            if (worldCells.Any(c => c.type != CellType.Free))
            {
                reason = $"tower overlaps path";
                return false;
            }

            if (worldCells.Intersect(_gameState.GetProcessorState().cells).Any())
            {
                reason = $"tower overlaps processor";
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
    }

    public static class TowerSpawnerApiExtensions
    {
        public static bool TrySpawnTower(this TowerSpawnerApi @this, TowerConfig tower, Vector2Int cell, bool force = false, bool register = true)
        {
            return @this.TrySpawnTower(tower, cell, out _, force, register);
        }
    }
}
