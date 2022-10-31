using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace GameComponents
{
    public class ProcessorController : MyMonoBehaviour
    {
        private float _currentTicks;
        
        void Update()
        {
            RequireGameManager();

            ProcessorState processorState = GameManager.gameState.processorState;
            if (processorState == null)
            {
                return;
            }
            
            processorState.ticks.Add(Time.deltaTime * processorState.config.frequency);
        
            // use SetCharge so that we can use a TowerAnimation instead of creating a new identical ProcessorAnimation behaviour
            gameObject.SendMessage("SetCharge", processorState.health);
        }
    }
}
