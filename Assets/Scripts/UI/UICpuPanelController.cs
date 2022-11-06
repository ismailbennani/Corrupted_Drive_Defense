using GameEngine.Processor;
using Utils.CustomComponents;

namespace UI
{
    public class UICpuPanelController : MyMonoBehaviour
    {
        public UIGaugeController healthBar;
        public UIGaugeController chargeBar;
        private ProcessorState _processorState;

        private void Update()
        {
            if (!TryGetGameManager())
            {
                return;
            }

            if ((_processorState ??= GameManager.GameState?.GetProcessorState()) == null)
            {
                return;
            }

            if (healthBar)
            {
                healthBar.Set(_processorState.health);
            }

            if (chargeBar)
            {
                chargeBar.Set(_processorState.charge);
            }
        }
    }
}
