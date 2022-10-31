using GameComponents;
using GameEngine.Towers;
using UnityEngine;

namespace UI
{
    public class UICpuPanelController : MonoBehaviour
    {
        public UIGaugeController healthBar;
        public UIGaugeController ticksBar;

        void Update()
        {
            GameManager gameManager = GameManager.Instance;
            if (!gameManager)
            {
                return;
            }

            ProcessorState processorState = gameManager.gameState.processorState;

            healthBar.Set(processorState.health);
            ticksBar.Set(processorState.ticks);
        }
    }
}
