using GameEngine.Map;
using GameEngine.Towers;
using Managers.Tower;
using UnityEngine.Assertions;
using Utils;

namespace Managers.Utils
{
  public class MouseInputApi
  {
    private readonly GameStateApi _gameStateApi;
    private readonly SelectedEntityApi _selectedEntityApi;

    private bool _disabled;

    public MouseInputApi(GameStateApi gameStateApi, SelectedEntityApi selectedEntityApi)
    {
      Assert.IsNotNull(gameStateApi);
      Assert.IsNotNull(selectedEntityApi);
      
      _gameStateApi = gameStateApi;
      _selectedEntityApi = selectedEntityApi;
    }

    public void Enable()
    {
      _disabled = false;
    }
    
    public void Disable()
    {
      _disabled = true;
    }

    public void Click()
    {
      if (_disabled)
      {
        return;
      }
      
      WorldCell cell = Mouse.GetTargetCell();

      TowerState tower = _gameStateApi.GetTowerAt(cell);
      if (tower != null)
      {
        _selectedEntityApi.Select(tower);
      }
      else if (_gameStateApi.IsProcessorCell(cell))
      {
        _selectedEntityApi.SelectProcessor();
      }
      else
      {
        _selectedEntityApi.Clear();
      }
    }
  }
}