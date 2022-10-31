using System;
using System.Collections;
using System.Linq;
using GameEngine.Towers;
using Utils.CustomComponents;

namespace GameComponents
{
    public class TowerController: MyMonoBehaviour
    {
        public long id;

        private TowerState _state;
        
        IEnumerator Start()
        {
            RequireGameManager();

            while (id <= 0)
            {
                yield return null;
            }

            if (id == GameManager.gameState.processorState.id)
            {
                Destroy(this);
                yield break;
            }
            
            _state = GameManager.gameState.towerStates.SingleOrDefault(t => t.id == id);
            if (_state == null)
            {
                throw new InvalidOperationException($"could not find state of tower with id {id}");
            }
        }
        
        void Update()
        {
            if (_state == null)
            {
                return;
            }
            
            SendMessage("SetCharge", _state.charge);   
        }
    }
}
