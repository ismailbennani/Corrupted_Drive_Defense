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
            int kills = 0;
            int hp = enemyState.characteristics.hp;
            EnemyConfig newConfig = enemyState.config;
            
            while (damage >= hp)
            {
                damage -= hp;
                kills++;
                
                if (newConfig.child == null)
                {
                    newConfig = null;
                    break;
                }

                newConfig = newConfig.child;
                hp = newConfig.hp;
            }

            if (kills > 0)
            {
                _gameStateApi.Earn(kills);
                Kill(enemyState.id);

                if (newConfig != null)
                {
                    enemyState.SetConfig(newConfig);
                    _enemySpawnApi.SpawnEnemy(enemyState);
                }
            }

            enemyState.characteristics.hp -= damage;
            
            return kills;
        }
    }
}
