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

            _state = GameManager.gameState.enemyStates.SingleOrDefault(t => t.id == id);
            if (_state == null)
            {
                throw new InvalidOperationException($"could not find enemy state with id {id}");
            }
            
            _path = GameManager.mapManager.GetPath();
            
            UpdateTargetPositions();
        }

        void FixedUpdate()
        {
            if (_state.pathCellCompletion > 1)
            {
                _state.pathIndex++;
                _state.pathCellCompletion -= 1;
                
                UpdateTargetPositions();
            }
            
            transform.position = _state.pathCellCompletion < 0.5
                ? Vector2.LerpUnclamped(_startPos, _middlePos, _state.pathCellCompletion * 2).WithDepth(transform.position.z)
                : Vector2.LerpUnclamped(_middlePos, _targetPos, _state.pathCellCompletion * 2 - 1).WithDepth(transform.position.z);

            _state.pathCellCompletion += Time.fixedDeltaTime * _state.config.speed;
        }

        private void UpdateTargetPositions()
        {
            WorldCell previousCell = _state.pathIndex == 0 ? _path[_state.pathIndex] : _path[_state.pathIndex - 1];
            WorldCell currentCell = _path[_state.pathIndex];
            WorldCell nextCell = _path[_state.pathIndex + 1];

            _startPos = currentCell.worldPosition - (currentCell.worldPosition - previousCell.worldPosition) / 2;
            _middlePos = currentCell.worldPosition;
            _targetPos = currentCell.worldPosition + (nextCell.worldPosition - currentCell.worldPosition) / 2;
        }
    }
}
