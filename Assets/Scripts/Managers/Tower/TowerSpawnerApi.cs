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
            WorldCell worldCell = _map.GetCellAt(cell);
            
            if (!force)
            {
                if (worldCell.type != CellType.Free)
                {
                    state = null;
                    return false;
                }
            }
            
            bool result = _towerSpawnerManager.SpawnTower(tower, worldCell, out state);

            if (!result)
            {
                return false;
            }
            
            if (register)
            {
                _gameState.AddTower(state);
            }

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
