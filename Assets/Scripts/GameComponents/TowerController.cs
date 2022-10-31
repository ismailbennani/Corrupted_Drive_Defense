using System;
using System.Collections;
using System.Linq;
using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace GameComponents
{
    public class TowerController: MyMonoBehaviour
    {
        public long id;

        private TowerState _state;
        
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
            if (_state.ticks.Full)
            {
                Debug.Log("Trigger");
                _state.ticks.Clear();
            }
        }

        private void UpdateCharge()
        {
            ProcessorState processorState = GameManager.gameState.processorState;

            float charge = Time.deltaTime * _state.config.frequency;
            float consumed = processorState.ticks.Consume(charge);

            _state.ticks.Add(consumed);
            SendMessage("SetCharge", _state.ticks);
        }
    }
}
