using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Utils
{
    public class KeyboardInputApi
    {
        private readonly GameManager _gameManager;

        public KeyboardInputApi(GameManager gameManager)
        {
            Assert.IsNotNull(gameManager);

            _gameManager = gameManager;
        }

        public void Update()
        {
            if (Input.GetButtonUp("Rotate"))
            {
                _gameManager.TowerSpawnPreview.ToggleRotation();
            }
        }
    }
}
