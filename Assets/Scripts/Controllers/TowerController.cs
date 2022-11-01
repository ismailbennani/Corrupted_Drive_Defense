﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies;
using GameEngine.Map;
using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace Controllers
{
    public class TowerController : MyMonoBehaviour
    {
        public long id;

        private TowerState _state;
        private ProcessorState _processorState;
        private WorldCell[] _path;

        IEnumerator Start()
        {
            RequireGameManager();

            while (id <= 0)
            {
                yield return null;
            }

            _state = GameManager.gameState.towerStates.SingleOrDefault(t => t.id == id);
            if (_state == null)
            {
                throw new InvalidOperationException($"could not find state of tower with id {id}");
            }

            _processorState = GameManager.gameState.processorState;
            if (_state == null)
            {
                throw new InvalidOperationException($"could not find state of processor");
            }

            _path = GameManager.Map.GetPath().ToArray();
        }

        void Update()
        {
            if (_state == null)
            {
                return;
            }

            TriggerIfPossible();
            UpdateCharge();
        }

        private void TriggerIfPossible()
        {
            if (!_state.ticks.Full)
            {
                return;
            }

            IEnumerable<Vector2Int> targetCells = _state.config.targetArea.EvaluateAt(_state.cell.gridPosition);
            int[] pathCells = targetCells.Select(c => Array.FindIndex(_path, w => w.gridPosition == c)).Where(i => i >= 0).ToArray();
            EnemyState[] targets = GameManager.gameState.enemyStates.Where(e => pathCells.Contains(e.pathIndex)).ToArray();

            if (targets.Length == 0)
            {
                return;
            }

            // TODO: pick target according to strategy
            EnemyState target = targets.First();

            _state.ticks.Clear();
            Debug.Log($"hit {target.config.enemyName} ({target.id})");
        }

        private void UpdateCharge()
        {
            float requiredCharge = _state.ticks.GetRemaining();
            float maxCharge = Time.deltaTime * _state.config.frequency;

            float consumed = _processorState.ticks.Consume(Mathf.Min(requiredCharge, maxCharge));

            _state.ticks.Add(consumed);
            SendMessage("SetCharge", _state.ticks);
        }
    }
}