using GameEngine.Tower;
using UnityEngine;
using Utils;
using Utils.Extensions;

public class TowerSpawnPreviewManager : MonoBehaviour
{
    public static TowerSpawnPreviewManager Instance { get; private set; }

    private TowerConfig _tower;
    private Transform _preview;
    private GameManager _gameManager;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        _gameManager = GameManager.Instance;
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
            Vector2Int? cell = Mouse.GridPosition();
            if (cell.HasValue)
            {
                SpawnAt(cell.Value);
            }
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

    public void SpawnAt(Vector2Int cell)
    {
        if (!_tower)
        {
            return;
        }

        Debug.Log($"Spawn {_tower.name} at {cell}");

        _gameManager.SpawnTower(_tower, cell);
        StopPreview();
    }

    private static Vector3 GetPreviewPosition()
    {
        return Mouse.WorldSpacePosition().WithDepth(-2);
    }
}
