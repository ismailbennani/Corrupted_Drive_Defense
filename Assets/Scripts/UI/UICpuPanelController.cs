using GameEngine.Processor;
using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

namespace UI
{
    public class UICpuPanelController : MyMonoBehaviour
    {
        public UIGaugeController healthBar;
        public UIGaugeController chargeBar;
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
            chargeBar.Set(_processorState.charge);
        }
    }
}
