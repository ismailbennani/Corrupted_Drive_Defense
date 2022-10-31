using GameEngine.Map;
using GameEngine.Tower;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Utils.Interfaces;

public class TowerSpawnPreviewManager : MonoBehaviour, INeedsGameManager
{
    public static TowerSpawnPreviewManager Instance { get; private set; }
    public GameManager GameManager { get; set; }

    private TowerConfig _tower;
    private Transform _preview;


    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!_tower)
        {
            return;
        }

        if (_preview)
        {
            _preview.position = GetPreviewPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            WorldCell cell = Mouse.GetTargetCell();
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

        if (!_preview)
        {
            _preview = new GameObject("SpawnPreview").transform;
            _preview.SetParent(transform);
        }
        
        _preview.gameObject.SetActive(true);
        _preview.position = GetPreviewPosition();

        _preview.RemoveAllChildren();
        Instantiate(tower.prefab, _preview);

        Debug.Log($"Start preview of {tower.name}");
    }

    public void StopPreview()
    {
        if (!_tower)
        {
            return;
        }

        Debug.Log($"Stop preview of {_tower.name}");

        _tower = null;

        if (_preview)
        {
            _preview.gameObject.SetActive(false);
        }
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

    private static Vector3 GetPreviewPosition()
    {
        return Mouse.GetTargetCell().worldPosition.WithDepth(GameConstants.UiLayer);
    }
}
