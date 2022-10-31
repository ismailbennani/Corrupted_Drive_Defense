using System.Linq;
using GameEngine.Enemies;
using UnityEngine;
using Utils.Interfaces;

namespace GameComponents
{
    public class EnemyController: MonoBehaviour, INeedsGameManager
    {
        public GameManager GameManager { get; set; }

        public long id;
        
        void Update()
        {
            if (id <= 0)
            {
                return;
            }
            
            this.RequireGameManager();

            if (GameManager.gameState.enemyStates.All(t => t.id != id))
            {
                return;
            }
            
            EnemyState state = GameManager.gameState.enemyStates.Single(t => t.id == id);   
        }
    }
}
