using GameEngine.Towers;
using Managers.Utils;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class TowerSpawnPreviewApi
    {
        private readonly TowerSpawnPreviewManager _towerSpawnPreviewManager;
        private readonly MouseInputApi _mouseInputApi;

        private bool _inPreview;

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
            _towerSpawnPreviewManager.OnStopPreview.AddListener(OnStopPreview);

            _inPreview = true;
        }

        public void StopPreview()
        {
            if (!_inPreview)
            {
                return;
            }
            
            _towerSpawnPreviewManager.StopPreview();
        }

        private void OnStopPreview()
        {
            _mouseInputApi.Enable();
            _towerSpawnPreviewManager.OnStopPreview.RemoveListener(OnStopPreview);
            _inPreview = false;
        }
    }
}
