using GameEngine.Map;
using GameEngine.Towers;
using Utils;
using Utils.CustomComponents;

namespace Managers.Utils
{
  public class MouseInputManager: MyMonoBehaviour
  {
    private void Start()
    {
      RequireGameManager();
    }

    public void Click()
    {
      WorldCell cell = Mouse.GetTargetCell();

      TowerState tower = GameManager.GameState.GetTowerAt(cell);
      if (tower != null)
      {
        GameManager.SelectTower(tower);
      }
      else
      {
        GameManager.UnselectTower();
      }
    }
  }
}