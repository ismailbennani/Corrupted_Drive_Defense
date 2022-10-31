using System;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using UnityEngine;
using Utils.Extensions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameConfig gameConfig;

    [Header("Created on start")]
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

    public void SpawnTower(TowerConfig tower, WorldCell cell)
    {
        TowerSpawnManager spawner = TowerSpawnManager.Instance;
        spawner.SpawnTower(tower, cell);
    }

    public void StartWave()
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

        mapManager = map.GetComponent<MapManager>();

        mapManager.mapConfig = gameConfig.mapConfig;
        mapManager.Initialize();
    }

    private void SpawnProcessor()
    {
        TowerSpawnManager spawner = TowerSpawnManager.Instance;
        if (!spawner)
        {
            throw new InvalidOperationException("could not find tower spawn manager");
        }

        WorldCell processorCell = mapManager.GetCellAt(gameConfig.mapConfig.processorPosition);

        if (!spawner.SpawnTower(gameConfig.processor, processorCell, out long id, force: true, register: false))
        {
            throw new InvalidOperationException("could not spawn processor");
        }
        
        gameState.processorState = new ProcessorState { id = id, cell = processorCell };
    }
}
