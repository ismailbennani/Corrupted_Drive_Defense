using System;
using System.Collections.Generic;
using GameEngine.Map;
using GameEngine.Towers;
using UnityEngine;
using Utils;
using Utils.CustomComponents;
using Utils.Extensions;

public class TowerSpawnPreviewManager : MyMonoBehaviour, INeedsComponent<VisibleShapeManager>
{
    public static TowerSpawnPreviewManager Instance { get; private set; }

    public Transform preview;

    [Header("Preview cell")]
    public SpriteRenderer cellPrefab;

    public Color okColor = Color.white;
    public Color errorColor = Color.red;

    private TowerConfig _tower;
    private Transform _previewTower;
    private List<SpriteRenderer> _previewCells = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!preview)
        {
            throw new InvalidOperationException("preview root not found");
        }
        
        RequireGameManager();
        this.RequireComponent<VisibleShapeManager>();
        
        StopPreview();
    }

    void Update()
    {
        if (!_tower)
        {
            return;
        }

        WorldCell cell = Mouse.GetTargetCell();
        preview.position = cell.worldPosition.WithDepth(GameConstants.UiLayer);
        _visibleShapeManager.SetPosition(cell.worldPosition);
        _visibleShapeManager.SetColor(cell.type == CellType.Free ? okColor : errorColor);

        if (Input.GetMouseButtonUp(0))
        {
            SpawnAt(cell);
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopPreview();
        }
    }

    public void StartPreview(TowerConfig tower)
    {
        _tower = tower;
        Debug.Log($"Start preview of {tower.towerName}");

        preview.gameObject.SetActive(true);

        if (_previewTower)
        {
            Destroy(_previewTower.gameObject);
        }

        _previewTower = Instantiate(tower.prefab, preview).transform;
        _visibleShapeManager.Show(tower.targetArea, _previewTower.position);
    }

    public void StopPreview()
    {
        if (_tower)
        {
            Debug.Log($"Stop preview of {_tower.towerName}");
        }

        _tower = null;
        preview.gameObject.SetActive(false);
        _visibleShapeManager.Hide();
    }

    public void SpawnAt(WorldCell cell)
    {
        if (cell.type != CellType.Free)
        {
            Debug.LogWarning($"Cannot build tower at {cell.gridPosition} because it is not free: {cell.type}");
            return;
        }

        GameManager.SpawnTower(_tower, cell);
        StopPreview();
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
