using GameEngine.Towers;

namespace Managers.Tower
{
    public class TowerSpawnPreviewApi
    {
        private TowerSpawnPreviewManager _towerSpawnPreviewManager;

        public TowerSpawnPreviewApi(TowerSpawnPreviewManager towerSpawnPreviewManager)
        {
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
