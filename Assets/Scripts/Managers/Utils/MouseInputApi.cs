using GameEngine.Map;
using GameEngine.Towers;
using Managers.Tower;
using Utils;

namespace Managers.Utils
{
  public class MouseInputApi
  {
    private readonly GameStateApi _gameStateApi;
    private readonly SelectedTowerApi _selectedTowerApi;

    public MouseInputApi(GameStateApi gameStateApi, SelectedTowerApi selectedTowerApi)
    {
      _gameStateApi = gameStateApi;
      _selectedTowerApi = selectedTowerApi;
    }

    public void Click()
    {
      WorldCell cell = Mouse.GetTargetCell();

      TowerState tower = _gameStateApi.GetTowerAt(cell);
      if (tower != null)
      {
        _selectedTowerApi.Select(tower);
      }
      else
      {
        _selectedTowerApi.Clear();
      }
    }
  }
}