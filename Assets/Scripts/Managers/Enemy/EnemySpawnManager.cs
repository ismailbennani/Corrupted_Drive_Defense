using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using GameEngine.Enemies;
using GameEngine.Map;
using Managers.Map;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Utils.Extensions;

namespace Managers.Enemy
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public GameStateApi GameState;
        public MapApi Map;

        private Transform _root;
        private readonly List<EnemyController> _enemies = new();
        
        void Start()
        {
            Assert.IsNotNull(GameState);
            Assert.IsNotNull(Map);

            _root = new GameObject("SpawnRoot").transform;
        }

        public void SpawnEnemy(EnemyConfig enemy, Vector2Int cell)
        {
            long id = Uid.Get();
            EnemyState newEnemyState = new(id, enemy);
            
            SpawnEnemy(newEnemyState);

            Debug.Log($"Spawn {enemy.enemyName} at spawn");
        }

        public void SpawnEnemy(EnemyState state)
        {
            if (!state.config || !state.config.prefab)
            {
                throw new InvalidOperationException("could not find enemy prefab");
            }

            GameState.AddEnemy(state);
            
            EnemyController newEnemy = Instantiate(state.config.prefab, Vector3.zero, Quaternion.identity, _root);
            newEnemy.id = state.id;
            _enemies.Add(newEnemy);
        }

        public void DestroyEnemy(long id)
        {
            EnemyController controller = _enemies.SingleOrDefault(c => c.id == id);
            if (controller == null)
            {
                Debug.LogWarning($"Could not find enemy controller with id {id}");
            }

            _enemies.Remove(controller);
            
            Destroy(controller.GameObject());
        }
    }
}
