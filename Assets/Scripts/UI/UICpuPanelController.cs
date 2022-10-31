using GameEngine.State;
using UnityEngine;

namespace UI
{
    public class UICpuPanelController : MonoBehaviour
    {
        public UIFillableBarController healthBar;
        public UIFillableBarController ticksBar;
    
        void Update()
        {
            GameManager gameManager = GameManager.Instance;
            if (!gameManager)
            {
                return;
            }

            ProcessorState processorState = gameManager.gameState.processorState;
        
            healthBar.SetValue(processorState.health, processorState.maxHealth);
            ticksBar.SetValue(processorState.ticks, processorState.maxTicks);
        }
    }
}
