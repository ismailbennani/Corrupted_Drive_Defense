using GameEngine.Towers;
using Utils.CustomComponents;

namespace GameComponents
{
    public class ProcessorController : MyMonoBehaviour
    {
        void Update()
        {
            RequireGameManager();

            ProcessorState processorState = GameManager.gameState.processorState;
        
            gameObject.SendMessage("SetCharge", new GaugeState(processorState.health, processorState.maxHealth));
        }
    }
}
