using System;
using Controllers;
using GameEngine.Enemies;
using GameEngine.Map;
using UnityEngine;
using Utils;
using Utils.CustomComponents;
using Utils.Extensions;

namespace Managers.Enemy
{
    public class EnemySpawnManager : MyMonoBehaviour
    {
        public Transform root;

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
    }
}
