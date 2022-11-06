using GameEngine.Towers;
using Managers.Utils;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class TowerSpawnPreviewApi
    {
        private readonly TowerSpawnPreviewManager _towerSpawnPreviewManager;
        private readonly MouseInputApi _mouseInputApi;

        public bool InPreview { get; private set; }

        public TowerSpawnPreviewApi(TowerSpawnPreviewManager towerSpawnPreviewManager, MouseInputApi mouseInputApi)
        {
            Assert.IsNotNull(towerSpawnPreviewManager);
            Assert.IsNotNull(mouseInputApi);

            _towerSpawnPreviewManager = towerSpawnPreviewManager;
            _mouseInputApi = mouseInputApi;
        }

        public void StartPreview(TowerConfig tower)
        {
            _mouseInputApi.Disable();
            _towerSpawnPreviewManager.StartPreview(tower);
            _towerSpawnPreviewManager.onStopPreview.AddListener(OnStopPreview);

            InPreview = true;
        }

        public void ToggleRotation()
        {
            _towerSpawnPreviewManager.ToggleRotated();
        }

        public void StopPreview()
        {
            if (!InPreview)
            {
                return;
            }

            _towerSpawnPreviewManager.StopPreview();
        }

        private void OnStopPreview()
        {
            _mouseInputApi.Enable();
            _towerSpawnPreviewManager.onStopPreview.RemoveListener(OnStopPreview);
            InPreview = false;
        }
    }
}
