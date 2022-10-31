using System;
using GameEngine.Map;
using GameEngine.Towers;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Utils.Interfaces;

public class TowerSpawnPreviewManager : MonoBehaviour, INeedsGameManager
{
    public static TowerSpawnPreviewManager Instance { get; private set; }
    public GameManager GameManager { get; set; }

    public Transform preview;

    [Header("Preview cell")]
    public SpriteRenderer previewCell;

    public Color okColor = Color.white;
    public Color errorColor = Color.red;

    private TowerConfig _tower;
    private Transform _previewTower;

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

        if (!previewCell)
        {
            throw new InvalidOperationException("preview cell not found");
        }

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

        if (cell.type == CellType.Free)
        {
            previewCell.color = okColor;
        }
        else
        {
            previewCell.color = errorColor;
        }

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
        Debug.Log($"Start preview of {tower.name}");

        preview.gameObject.SetActive(true);

        if (_previewTower)
        {
            Destroy(_previewTower.gameObject);
        }

        _previewTower = Instantiate(tower.prefab, preview).transform;
    }

    public void StopPreview()
    {
        if (_tower)
        {
            Debug.Log($"Stop preview of {_tower.name}");
        }

        _tower = null;
        preview.gameObject.SetActive(false);
    }

    public void SpawnAt(WorldCell cell)
    {
        if (!_tower || !this.TryGetGameManager())
        {
            return;
        }

        if (cell.type != CellType.Free)
        {
            Debug.LogWarning($"Cannot build tower at {cell.gridPosition} because it is not free: {cell.type}");
            return;
        }

        GameManager.SpawnTower(_tower, cell);
        StopPreview();
    }
}
