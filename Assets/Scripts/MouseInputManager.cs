using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using Utils;
using Utils.CustomComponents;

public class MouseInputManager: MyMonoBehaviour
{
  private void Start()
  {
    RequireGameManager();
  }

  public void Click()
  {
    WorldCell cell = Mouse.GetTargetCell();

    GameState gameState = GameManager.gameState;

    foreach (TowerState tower in gameState.towerStates)
    {
      if (cell.gridPosition != tower.cell.gridPosition)
      {
        continue;
      }
      
      GameManager.SelectTower(tower);
      return;
    }
    
    GameManager.UnselectTower();
  }
}