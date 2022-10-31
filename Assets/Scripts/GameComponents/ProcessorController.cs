using GameEngine.Tower;
using UnityEngine;

namespace GameComponents
{
    public class ProcessorController : MonoBehaviour
    {
        public GameManager gameManager;

        void Update()
        {
            if (!GetGameManager())
            {
                return;
            }

            ProcessorState processorState = gameManager.gameState.processorState;
        
            gameObject.SendMessage("SetCharge", new GaugeState(processorState.health, processorState.maxHealth));
        }

        private bool GetGameManager()
        {
            if (!gameManager)
            {
                gameManager = GameManager.Instance;
            }

            return gameManager;
        }
    }
}
