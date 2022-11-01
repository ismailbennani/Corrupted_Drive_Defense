using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace UI
{
    public class UICpuPanelController : MyMonoBehaviour
    {
        public UIGaugeController healthBar;
        public UIGaugeController ticksBar;

        void Update()
        {
            if (!TryGetGameManager())
            {
                return;
            }

            ProcessorState processorState = GameManager.gameState?.processorState;
            if (processorState == null)
            {
                return;
            }

            healthBar.Set(processorState.health);
            ticksBar.Set(processorState.ticks);
        }
    }
}
