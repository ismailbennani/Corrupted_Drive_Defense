using GameEngine.Processor;
using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace Controllers
{
    public class ProcessorController : MyMonoBehaviour
    {
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
            
            _processorState.charge.Add(Time.deltaTime * _processorState.description.chargeRate);

            GameObject localGameObject = gameObject;
            localGameObject.SendMessage("SetHealth", _processorState.health);
            localGameObject.SendMessage("SetCharge", _processorState.charge);
        }
    }
}
