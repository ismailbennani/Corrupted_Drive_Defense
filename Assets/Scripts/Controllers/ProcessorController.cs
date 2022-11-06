using GameEngine.Processor;
using UnityEngine;
using Utils.CustomComponents;

namespace Controllers
{
    public class ProcessorController : MyMonoBehaviour
    {
        private ProcessorState _processorState;

        private void Start()
        {
            RequireGameManager();
        }

        private void Update()
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
