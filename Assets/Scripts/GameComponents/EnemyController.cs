using System.Linq;
using GameEngine.Enemies;
using Utils.CustomComponents;

namespace GameComponents
{
    public class EnemyController: MyMonoBehaviour
    {
        public long id;
        
        void Update()
        {
            if (id <= 0)
            {
                return;
            }
            
            RequireGameManager();

            if (GameManager.gameState.enemyStates.All(t => t.id != id))
            {
                return;
            }
            
            EnemyState state = GameManager.gameState.enemyStates.Single(t => t.id == id);   
        }
    }
}
