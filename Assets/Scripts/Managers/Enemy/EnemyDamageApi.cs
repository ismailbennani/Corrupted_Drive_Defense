﻿using GameEngine.Enemies;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Enemy
{
    public class EnemyDamageApi
    {
        private readonly GameStateApi _gameStateApi;
        private readonly EnemySpawnApi _enemySpawnApi;

        public EnemyDamageApi(GameStateApi gameStateApi, EnemySpawnApi enemySpawnApi)
        {
            Assert.IsNotNull(gameStateApi);
            Assert.IsNotNull(enemySpawnApi);
            
            _gameStateApi = gameStateApi;
            _enemySpawnApi = enemySpawnApi;
        }

        public void Hit(long enemyId, int damage, out int kill)
        {
            kill = 0;

            EnemyState enemyState = _gameStateApi.GetEnemyState(enemyId);
            if (enemyState == null)
            {
                Debug.LogWarning($"Could not find enemy with id {enemyId}");
                return;
            }

            enemyState.hp -= damage;
            if (enemyState.hp <= 0)
            {
                kill = 1;
                _enemySpawnApi.DestroyEnemy(enemyState.id);

                if (enemyState.config.child != null)
                {
                    enemyState.config = enemyState.config.child;
                    _enemySpawnApi.SpawnEnemy(enemyState);
                }
                else
                {
                    _gameStateApi.RemoveEnemy(enemyId);
                }
            }
        }
    }
}
