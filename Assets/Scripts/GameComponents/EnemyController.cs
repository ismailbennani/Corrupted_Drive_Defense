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
                WorldCell currentCell = path[i];
                WorldCell nextCell = path[i + 1];

                state.pathIndex = i;

                Vector2 dir = nextCell.worldPosition - currentCell.worldPosition;
                Vector2 startPos = currentCell.worldPosition - dir / 2;
                Vector2 targetPos = currentCell.worldPosition + dir / 2;

                float progression = 0;
                while (progression <= 1)
                {
                    transform.position = Vector2.Lerp(startPos, targetPos, progression).WithDepth(transform.position.z);

                    progression += Time.fixedDeltaTime * state.config.speed;
                    state.pathCellCompletion = progression;

                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}
