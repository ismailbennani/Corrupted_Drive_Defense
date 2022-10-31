using System;
using GameEngine;
using GameEngine.Map;
using GameEngine.State;
using GameEngine.Tower;
using UnityEngine;
using Utils;
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

    public void SpawnTower(TowerConfig tower, WorldCell cell)
    {
        TowerSpawnManager spawner = TowerSpawnManager.Instance;

        long id = Uid.Get();
        if (!spawner.SpawnTower(id, tower, cell))
        {
            return;
        }

        TowerState newTowerState = new() { id = id, cell = cell };

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
        TowerSpawnManager spawner = TowerSpawnManager.Instance;
        if (!spawner)
        {
            throw new InvalidOperationException("could not find tower spawn manager");
        }

        WorldCell processorCell = mapManager.GetCellAt(gameConfig.mapConfig.processorPosition);

        long id = Uid.Get();
        spawner.SpawnTower(id, gameConfig.processor, processorCell, force: true);
        gameState.processorState = new ProcessorState { id = id, cell = processorCell };
    }
}
