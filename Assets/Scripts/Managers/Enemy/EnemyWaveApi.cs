using GameEngine;
using GameEngine.Waves;
using UnityEngine;

namespace Managers.Enemy
{
    public class EnemyWaveApi
    {
        private readonly GameConfig _gameConfig;
        private readonly GameStateApi _gameStateApi;
        private readonly EnemyWaveManager _enemyWaveManager;

        public EnemyWaveApi(GameConfig gameConfig, GameStateApi gameStateApi, EnemyWaveManager enemyWaveManager)
        {
            _gameConfig = gameConfig;
            _gameStateApi = gameStateApi;
            _enemyWaveManager = enemyWaveManager;

            _enemyWaveManager.EnemyWaveApi = this;
        }

        public void SpawnNextWave()
        {
            if (!_enemyWaveManager.Ready)
            {
                Debug.LogWarning("Next wave not ready");
                return;
            }
            
            int nextWaveIndex = _gameStateApi.GetCurrentWave();
            if (nextWaveIndex >= _gameConfig.waves.Length)
            {
                return;
            }

            WaveConfig wave = _gameConfig.waves[nextWaveIndex];
            _enemyWaveManager.SpawnWave(wave);
            
            _gameStateApi.IncrementWave();
        }

        public void SetAutoWave(bool auto)
        {
            _enemyWaveManager.SetAutoWave(auto);
        }

        public bool GetAutoWave()
        {
            return _enemyWaveManager.AutoWave;
        }
    }
}
