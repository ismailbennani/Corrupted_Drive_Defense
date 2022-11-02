using GameEngine.Map;
using GameEngine.Towers;
using Managers.Tower;
using UnityEngine.Assertions;
using Utils;

namespace Managers.Utils
{
  public class MouseInputApi
  {
    private readonly GameManager _gameManager;

    private bool _disabled;

    public MouseInputApi(GameManager gameManager)
    {
      Assert.IsNotNull(gameManager);
      
      _gameManager = gameManager;
    }

    public void Update()
    {
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

      TowerState tower = _gameManager.GameState.GetTowerAt(cell);
      if (tower != null)
      {
        _gameManager.SelectedEntity.Select(tower);
      }
      else if (_gameManager.GameState.IsProcessorCell(cell))
      {
        _gameManager.SelectedEntity.SelectProcessor();
      }
      else
      {
        _gameManager.SelectedEntity.Clear();
      }
    }
  }
}