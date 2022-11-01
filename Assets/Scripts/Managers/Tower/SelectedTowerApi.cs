using GameEngine.Towers;

namespace Managers.Tower
{
    public class SelectedTowerApi
    {
        private readonly SelectedTowerManager _selectedTowerManager;

        public SelectedTowerApi(SelectedTowerManager selectedTowerManager)
        {
            _selectedTowerManager = selectedTowerManager;
        }

        public TowerState Get()
        {
            return _selectedTowerManager.selectedTower;
        }

        public void Select(TowerState tower)
        {
            _selectedTowerManager.Select(tower);
        }

        public void Unselect(TowerState tower)
        {
            _selectedTowerManager.Unselect(tower);
        }

        public void Clear()
        {
            _selectedTowerManager.Clear();
        }
    }
}
