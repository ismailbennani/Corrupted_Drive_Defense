using GameEngine.Towers;
using UnityEngine.Assertions;

namespace Managers.Tower
{
    public class TowerSpawnPreviewApi
    {
        private readonly TowerSpawnPreviewManager _towerSpawnPreviewManager;

        public TowerSpawnPreviewApi(TowerSpawnPreviewManager towerSpawnPreviewManager)
        {
            Assert.IsNotNull(towerSpawnPreviewManager);
            
            _towerSpawnPreviewManager = towerSpawnPreviewManager;
        }

        public void StartPreview(TowerConfig tower)
        {
            _towerSpawnPreviewManager.StartPreview(tower);
        }

        public void StopPreview()
        {
            _towerSpawnPreviewManager.StopPreview();
        }
    }
}
