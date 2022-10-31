using System.Linq;
using GameEngine.Towers;
using Utils.CustomComponents;

namespace GameComponents
{
    public class TowerController: MyMonoBehaviour
    {
        public long id;

        void Update()
        {
            if (id <= 0)
            {
                return;
            }
            
            RequireGameManager();

            if (GameManager.gameState.towerStates.All(t => t.id != id))
            {
                return;
            }
            
            TowerState state = GameManager.gameState.towerStates.Single(t => t.id == id);
            
            SendMessage("SetCharge", state.charge);   
        }
    }
}
