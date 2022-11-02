using System;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Map;
using UnityEngine;
using Utils.CustomComponents;
using Utils.Extensions;

namespace Controllers
{
    public class EnemyController : MyMonoBehaviour
    {
        public long id;

        private EnemyState _state;
        private WorldCell[] _path;

        private Vector2 _startPos;
        private Vector2 _middlePos;
        private Vector2 _targetPos;

        void Start()
        {
            if (id <= 0)
            {
                throw new InvalidOperationException($"enemy with invalid id {id}");
            }

            RequireGameManager();

            _state = GameManager.GameState.GetEnemyState(id);
            if (_state == null)
            {
                throw new InvalidOperationException($"could not find enemy state with id {id}");
            }
            
            _path = GameManager.Map.GetPath().ToArray();
            
            UpdateTargetPositions();
        }

        void FixedUpdate()
        {
            _state.Update();
            
            if (_state.pathCellCompletion > 1)
            {
                _state.pathIndex++;

                if (_state.pathIndex >= _path.Length)
                {
                    GameManager.Enemy.Kill(_state.id);
                    GameManager.ProcessorDamage.Hit(_state);
                }
                
                _state.pathCellCompletion -= 1;
                
                UpdateTargetPositions();
            }
            
            transform.position = _state.pathCellCompletion < 0.5
                ? Vector2.LerpUnclamped(_startPos, _middlePos, _state.pathCellCompletion * 2).WithDepth(transform.position.z)
                : Vector2.LerpUnclamped(_middlePos, _targetPos, _state.pathCellCompletion * 2 - 1).WithDepth(transform.position.z);

            _state.pathCellCompletion += Time.fixedDeltaTime * _state.characteristics.speed;
        }

        private void UpdateTargetPositions()
        {
            WorldCell previousCell = _state.pathIndex <= 0 ? _path[_state.pathIndex] : _path[_state.pathIndex - 1];
            WorldCell currentCell = _state.pathIndex >= _path.Length ? _path[^1] : _path[_state.pathIndex];
            WorldCell nextCell = _state.pathIndex >= _path.Length - 1 ? _path[^1] : _path[_state.pathIndex + 1];

            _startPos = currentCell.worldPosition - (currentCell.worldPosition - previousCell.worldPosition) / 2;
            _middlePos = currentCell.worldPosition;
            _targetPos = currentCell.worldPosition + (nextCell.worldPosition - currentCell.worldPosition) / 2;
        }
    }
}
