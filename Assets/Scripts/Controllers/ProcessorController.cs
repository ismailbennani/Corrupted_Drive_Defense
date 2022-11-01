using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace Controllers
{
    public class ProcessorController : MyMonoBehaviour
    {
        private float _currentTicks;
        private ProcessorState _processorState;

        void Start()
        {
            RequireGameManager();
        }
        
        void Update()
        {
            if ((_processorState ??= GameManager.GameState.GetProcessorState()) == null)
            {
                return;
            }
            
            _processorState.ticks.Add(Time.deltaTime * _processorState.config.frequency);
        
            // use SetCharge so that we can use a TowerAnimation instead of creating a new identical ProcessorAnimation behaviour
            gameObject.SendMessage("SetCharge", _processorState.health);
        }
    }
}
