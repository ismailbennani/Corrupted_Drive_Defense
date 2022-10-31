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
        private ProcessorState _processorState;
        
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
        }

        private void UpdateCharge()
        {
            float requiredCharge = _state.ticks.GetRemaining();
            float maxCharge = Time.deltaTime * _state.config.frequency;
            
            float consumed = _processorState.ticks.Consume(Mathf.Min(requiredCharge, maxCharge));

            _state.ticks.Add(consumed);
            SendMessage("SetCharge", _state.ticks);
            
            Debug.Log(consumed);
        }
    }
}
