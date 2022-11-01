using GameEngine.Map;
using GameEngine.Towers;

namespace Managers.Tower
{
    public class TowerSpawnerApi
    {
        private readonly TowerSpawnerManager _towerSpawnerManager;
        private readonly GameStateApi _gameState;

        public TowerSpawnerApi(TowerSpawnerManager towerSpawnerManager, GameStateApi gameState)
        {
            _towerSpawnerManager = towerSpawnerManager;
            _gameState = gameState;
        }

        public bool SpawnTower(TowerConfig tower, WorldCell cell, out TowerState state, bool force = false, bool register = true)
        {
            bool result = _towerSpawnerManager.SpawnTower(tower, cell, out state, force);

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
        public static bool SpawnTower(this TowerSpawnerApi @this, TowerConfig tower, WorldCell cell, bool force = false, bool register = true)
        {
            return @this.SpawnTower(tower, cell, out _, force, register);
        }
    }
}
