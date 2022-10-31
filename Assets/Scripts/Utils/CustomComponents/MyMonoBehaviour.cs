using System;
using UnityEngine;

namespace Utils.CustomComponents
{
    public class MyMonoBehaviour : MonoBehaviour, INeedsComponent<GameManager>
    {
        public GameManager GameManager { get; set; }










        GameManager INeedsComponent<GameManager>.Component {
            get => GameManager;
            set => GameManager = value;
        }










        public GameManager GetInstance()
        {
            return GameManager.Instance;
        }

        protected bool TryGetGameManager()
        {
            return this.TryGetComponent<GameManager>();
        }

        protected void RequireGameManager()
        {
            this.RequireComponent<GameManager>();
        }
    }
}
