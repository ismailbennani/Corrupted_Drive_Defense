using GameEngine.Enemies;
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
    }
}
