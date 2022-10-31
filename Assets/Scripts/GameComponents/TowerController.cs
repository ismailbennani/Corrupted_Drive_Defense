using System.Linq;
using GameEngine.Tower;
using UnityEngine;
using Utils.Interfaces;

namespace GameComponents
{
    public class TowerController: MonoBehaviour, INeedsGameManager
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

            if (GameManager.gameState.towerStates.All(t => t.id != id))
            {
                return;
            }
            
            TowerState state = GameManager.gameState.towerStates.Single(t => t.id == id);
            
            SendMessage("SetCharge", state.charge);   
        }
    }
}
