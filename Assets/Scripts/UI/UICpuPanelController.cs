using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace UI
{
    public class UICpuPanelController : MyMonoBehaviour
    {
        public UIGaugeController healthBar;
        public UIGaugeController ticksBar;
        private ProcessorState _processorState;

        void Update()
        {
            if (!TryGetGameManager())
            {
                return;
            }

            if ((_processorState ??= GameManager.GameState?.GetProcessorState()) == null)
            {
                return;
            }

            healthBar.Set(_processorState.health);
            ticksBar.Set(_processorState.ticks);
        }
    }
}
