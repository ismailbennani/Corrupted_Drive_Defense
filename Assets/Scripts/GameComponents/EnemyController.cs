using System;
using System.Collections;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Map;
using UnityEngine;
using Utils.CustomComponents;
using Utils.Extensions;

namespace GameComponents
{
    public class EnemyController : MyMonoBehaviour
    {
        public long id;

        void Start()
        {
            StartCoroutine(MoveAlongPath());
        }

        private IEnumerator MoveAlongPath()
        {
            if (id <= 0)
            {
                throw new InvalidOperationException($"enemy with invalid id {id}");
            }

            RequireGameManager();

            EnemyState state = GameManager.gameState.enemyStates.SingleOrDefault(t => t.id == id);
            if (state == null)
            {
                throw new InvalidOperationException($"could not find enemy state with id {id}");
            }

            WorldCell[] path = GameManager.mapManager.GetPath();

            for (int i = 0; i < path.Length - 1; i++)
            {
                WorldCell previousCell = i == 0 ? path[i] : path[i - 1];
                WorldCell currentCell = path[i];
                WorldCell nextCell = path[i + 1];

                state.pathIndex = i;

                Vector2 startPos = currentCell.worldPosition - (currentCell.worldPosition - previousCell.worldPosition) / 2;
                Vector2 middlePos = currentCell.worldPosition;
                Vector2 targetPos = currentCell.worldPosition + (nextCell.worldPosition - currentCell.worldPosition) / 2;

                float progression = 0;
                while (progression <= 1)
                {
                    transform.position = progression < 0.5
                        ? Vector2.Lerp(startPos, middlePos, progression * 2).WithDepth(transform.position.z)
                        : Vector2.Lerp(middlePos, targetPos, progression * 2 - 1).WithDepth(transform.position.z);

                    progression += Time.fixedDeltaTime * state.config.speed;
                    state.pathCellCompletion = progression;

                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}
