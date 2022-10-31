using System;
using GameEngine.Towers;
using UnityEngine;
using Utils.CustomComponents;

public class SelectedTowerManager : MonoBehaviour, INeedsComponent<VisibleShapeManager>
{
    public static SelectedTowerManager Instance { get; private set; }
    
    public TowerState selectedTower;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        this.RequireComponent<VisibleShapeManager>();
    }

    public void Select(TowerState tower)
    {
        selectedTower = tower;
        _visibleShapeManager.Show(tower.config.targetArea, tower.cell.worldPosition);
    }

    public void Unselect(TowerState tower)
    {
        if (selectedTower.id == tower.id)
        {
            Unselect();
        }
    }

    public void Unselect()
    {
        selectedTower = null;
        _visibleShapeManager.Hide();
    }

    #region Needed components

    private VisibleShapeManager _visibleShapeManager;










    VisibleShapeManager INeedsComponent<VisibleShapeManager>.Component {
        get => _visibleShapeManager;
        set => _visibleShapeManager = value;
    }










    public VisibleShapeManager GetInstance()
    {
        return VisibleShapeManager.Instance;
    }

    #endregion
}
