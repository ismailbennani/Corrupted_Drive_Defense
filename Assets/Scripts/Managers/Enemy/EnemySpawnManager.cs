using System;
using System.Collections;
using Controllers;
using GameEngine.Enemies;
using GameEngine.Map;
using GameEngine.Waves;
using UnityEngine;
using Utils;
using Utils.CustomComponents;
using Utils.Extensions;

namespace Managers.Enemy
{
    public class EnemySpawnManager : MyMonoBehaviour
    {
        public bool Ongoing { get; private set; }
        public bool Ready => !Ongoing;

        public Transform root;
        public Vector2Int spawn;

        public void SpawnWave(WaveConfig wave)
        {
            StartCoroutine(SpawnWaveCoroutine(wave));
        }

        public void SpawnEnemy(EnemyConfig enemy, Vector2Int cell)
        {
            if (!enemy || !enemy.prefab)
            {
                throw new InvalidOperationException("could not find enemy prefab");
            }

            RequireGameManager();
        
            if (!root)
            {
                throw new InvalidOperationException("enemies root not set");
            }

            long id = Uid.Get();
            EnemyState newEnemyState = new(id, enemy);
            GameManager.GameState.AddEnemy(newEnemyState);

            WorldCell spawnCell = GameManager.Map.GetCellAt(cell);
            EnemyController newEnemy = Instantiate(enemy.prefab, Vector3.zero, Quaternion.identity, root);
            newEnemy.transform.localPosition = spawnCell.worldPosition.WithDepth(GameConstants.EntityLayer);
            newEnemy.id = id;
        
            Debug.Log($"Spawn {enemy.enemyName} at {spawnCell.gridPosition}");
        }

        private IEnumerator SpawnWaveCoroutine(WaveConfig wave)
        {
            Ongoing = true;

            foreach (EnemyConfig enemy in wave.enemies)
            {
                SpawnEnemy(enemy, spawn);
                yield return new WaitForSeconds(1f);
            }

            Ongoing = false;
        }
    }
}
