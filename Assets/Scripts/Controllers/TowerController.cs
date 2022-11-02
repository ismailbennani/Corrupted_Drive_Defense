using System;
using System.Collections;
using System.Linq;
using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Towers;
using Managers.Tower;
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

            _state = GameManager.GameState.GetTowerState(id);
            if (_state == null)
            {
                throw new InvalidOperationException($"could not find state of tower with id {id}");
            }

            _processorState = GameManager.GameState.GetProcessorState();
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

            GameManager.Tower.TriggerIfPossible(_state);
            
            UpdateCharge();
        }

        private void UpdateCharge()
        {
            float requiredCharge = _state.charge.GetRemaining();
            float maxCharge = Time.deltaTime * _state.config.frequency;

            float consumed = _processorState.charge.Consume(Mathf.Min(requiredCharge, maxCharge));

            _state.charge.Add(consumed);
            SendMessage("SetCharge", _state.charge);
        }
    }
}
