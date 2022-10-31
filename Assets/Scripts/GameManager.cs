using System;
using System.Collections.Generic;
using GameEngine;
using GameEngine.State;
using GameEngine.Tower;
using UnityEngine;
using Utils.Extensions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameConfig gameConfig;

    [Header("Set on start")]
    public GameState gameState;

    public MapManager mapManager;

    void Awake()
    {
        Instance = this;

        if (!gameConfig)
        {
            throw new InvalidOperationException("game config not set");
        }

        gameState = new GameState();
    }

    void Start()
    {
        this.RemoveAllChildren();

        SpawnMap();
        SpawnProcessor();
    }

    public void StartSpawning(TowerConfig tower)
    {
        TowerSpawnPreviewManager.Instance.StartPreview(tower);
    }

    public void SpawnTower(TowerConfig tower, Vector2Int cell)
    {
        SpawnTowerInternal(tower, cell);
        TowerState newTowerState = new() { position = cell, charge = 0, maxCharge = 0 };

        gameState.towerStates.Add(newTowerState);
    }

    private void SpawnMap()
    {
        if (!gameConfig.mapConfig)
        {
            throw new InvalidOperationException("map config not set");
        }


        Transform map = new GameObject("MapManager", typeof(MapManager)).transform;
        map.parent = transform;

        mapManager = map.GetComponent<MapManager>();

        mapManager.mapConfig = gameConfig.mapConfig;
        mapManager.Initialize();
    }

    private void SpawnProcessor()
    {
        SpawnTowerInternal(gameConfig.processor, gameConfig.mapConfig.processorPosition);
        gameState.processorState = new ProcessorState() { position = gameConfig.mapConfig.processorPosition };
    }

    private void SpawnTowerInternal(TowerConfig tower, Vector2Int cell)
    {
        if (!tower || !tower.prefab)
        {
            throw new InvalidOperationException("tower prefab not set");
        }

        Vector3 position = mapManager.GridCellToWorldPosition(cell);

        Instantiate(tower.prefab, position, Quaternion.identity, transform);
    }
}
