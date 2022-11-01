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

        public int GetRemaining()
        {
            return _enemySpawnManager.Remaining;
        }
        
        public void SpawnEnemy(EnemyConfig enemy, Vector2Int cell)
        {
            _enemySpawnManager.SpawnEnemy(enemy, cell);
        }

        public void SpawnEnemy(EnemyState state)
        {
            _enemySpawnManager.SpawnEnemy(state);
        }

        public void DestroyEnemy(long id)
        {
            _enemySpawnManager.DestroyEnemy(id);
        }
    }
}
