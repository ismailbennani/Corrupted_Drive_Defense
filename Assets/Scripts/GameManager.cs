using System;
using GameEngine;
using UnityEngine;
using Utils.Extensions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameConfig gameConfig;

    private MapManager _mapManager;

    void Awake()
    {
        Instance = this;
        
        if (!gameConfig)
        {
            throw new InvalidOperationException("game config not set");
        }
    }

    void Start()
    {
        this.RemoveAllChildren();

        SpawnMap();
        SpawnProcessor();
    }

    void Update()
    {

    }

    private void SpawnMap()
    {
        if (!gameConfig.mapConfig)
        {
            throw new InvalidOperationException("map config not set");
        }


        Transform map = new GameObject("MapManager", typeof(MapManager)).transform;
        map.parent = transform;

        _mapManager = map.GetComponent<MapManager>();

        _mapManager.mapConfig = gameConfig.mapConfig;
        _mapManager.Initialize();
    }

    private void SpawnProcessor()
    {
        if (!gameConfig.processor || !gameConfig.processor.prefab)
        {
            throw new InvalidOperationException("processor prefab not set");
        }

        Vector2Int processorCell = gameConfig.mapConfig.processorPosition;
        Vector3 position = _mapManager.GridCellToWorldPosition(processorCell);

        Instantiate(gameConfig.processor.prefab, position, Quaternion.identity, transform);
    }
}
