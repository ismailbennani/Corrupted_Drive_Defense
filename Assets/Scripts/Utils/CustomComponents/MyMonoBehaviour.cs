using Managers;
using UnityEngine;

namespace Utils.CustomComponents
{
    public class MyMonoBehaviour : MonoBehaviour, INeedsComponent<GameManager>
    {
        public GameManager GameManager {
            get {
                this.RequireComponent();
                return _gameManager;
            }
        }










        private GameManager _gameManager;










        GameManager INeedsComponent<GameManager>.Component {
            get => _gameManager;
            set => _gameManager = value;
        }










        public GameManager GetInstance()
        {
            return GameManager.Instance;
        }

        protected bool TryGetGameManager()
        {
            return this.TryGetNeededComponent();
        }
    }
}
