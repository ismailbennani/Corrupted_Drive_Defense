using System;
using Controllers;
using GameEngine.Enemies;
using GameEngine.Map;
using Managers.Map;
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
        
        void Start()
        {
            Assert.IsNotNull(GameState);
            Assert.IsNotNull(Map);

            _root = new GameObject("SpawnRoot").transform;
        }

        public void SpawnEnemy(EnemyConfig enemy, Vector2Int cell)
        {
            if (!enemy || !enemy.prefab)
            {
                throw new InvalidOperationException("could not find enemy prefab");
            }

            long id = Uid.Get();
            EnemyState newEnemyState = new(id, enemy);
            GameState.AddEnemy(newEnemyState);

            WorldCell spawnCell = Map.GetCellAt(cell);
            EnemyController newEnemy = Instantiate(enemy.prefab, Vector3.zero, Quaternion.identity, _root);
            newEnemy.transform.localPosition = spawnCell.worldPosition.WithDepth(GameConstants.EntityLayer);
            newEnemy.id = id;

            Debug.Log($"Spawn {enemy.enemyName} at {spawnCell.gridPosition}");
        }
    }
}
