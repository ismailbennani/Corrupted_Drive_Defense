using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Towers;
using UnityEngine.Assertions;

namespace Managers.Enemy
{
    public class EnemyApi
    {
        private readonly GameStateApi _gameStateApi;
        private readonly EnemySpawnApi _enemySpawnApi;

        public EnemyApi(GameStateApi gameStateApi, EnemySpawnApi enemySpawnApi)
        {
            Assert.IsNotNull(gameStateApi);
            Assert.IsNotNull(enemySpawnApi);

            _gameStateApi = gameStateApi;
            _enemySpawnApi = enemySpawnApi;
        }

        public void Hit(IEnumerable<long> enemyIds, int damage, TowerState source)
        {
            EnemyState[] enemyStates = enemyIds.Select(enemyId => _gameStateApi.GetEnemyState(enemyId)).ToArray();
            if (!enemyStates.Any())
            {
                return;
            }

            int kills = enemyStates.Aggregate(0, (kills, e) => kills + Hit(e, damage));
            _gameStateApi.AddKills(source, kills);
        }

        public void Kill(long id)
        {
            EnemyState enemyState = _gameStateApi.GetEnemyState(id);
            _enemySpawnApi.DestroyEnemy(enemyState.id);
            _gameStateApi.RemoveEnemy(id);
        }

        private int Hit(EnemyState enemyState, int damage)
        {
            enemyState.hp -= damage;
            if (enemyState.hp <= 0)
            {
                int kills = 1;
                
                _gameStateApi.Earn(kills);
                Kill(enemyState.id);

                if (enemyState.config.child != null)
                {
                    _enemySpawnApi.DestroyEnemy(enemyState.id);
                    enemyState.config = enemyState.config.child;
                    _enemySpawnApi.SpawnEnemy(enemyState);
                }

                return kills;
            }

            return 0;
        }
    }
}
