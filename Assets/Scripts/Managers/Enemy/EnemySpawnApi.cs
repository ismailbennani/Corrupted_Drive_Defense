using GameEngine.Enemies;
using GameEngine.Waves;
using UnityEngine;

namespace Managers.Enemy
{
    public class EnemySpawnApi
    {
        private readonly EnemySpawnManager _enemySpawnManager;

        public EnemySpawnApi(EnemySpawnManager enemySpawnManager)
        {
            _enemySpawnManager = enemySpawnManager;
        }

        public void SpawnEnemy(EnemyConfig enemy, Vector2Int cell)
        {
            _enemySpawnManager.SpawnEnemy(enemy, cell);
        }

        public void SpawnWave(WaveConfig wave)
        {
            _enemySpawnManager.SpawnWave(wave);
        }

        public void SetAutoWave(bool auto)
        {
            
        }
    }
}
