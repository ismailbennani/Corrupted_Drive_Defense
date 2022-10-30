using System;
using GameEngine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameConfig gameConfig;

    private MapManager _mapManager;

    void Start()
    {
        if (!gameConfig)
        {
            throw new InvalidOperationException("game config not set");
        }

        RemoveChildren();

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

    private void RemoveChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
