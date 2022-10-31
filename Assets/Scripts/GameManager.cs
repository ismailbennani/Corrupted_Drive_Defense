using System;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using GameEngine.Waves;
using UnityEngine;
using Utils.CustomComponents;
using Utils.Extensions;

public class GameManager : MonoBehaviour, INeedsComponent<TowerSpawnManager>, INeedsComponent<TowerSpawnPreviewManager>, INeedsComponent<EnemySpawnManager>
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
        if (!this.TryGetNeededComponent<TowerSpawnPreviewManager>())
        {
            return;
        }

        _towerSpawnPreviewManager.StartPreview(tower);
    }

    public void SpawnTower(TowerConfig tower, WorldCell cell)
    {
        this.RequireComponent<TowerSpawnManager>();
        _towerSpawnManager.SpawnTower(tower, cell);
    }

    public void StartWave()
    {
        if (!this.TryGetNeededComponent<EnemySpawnManager>())
        {
            return;
        }

        WaveConfig currentWave = gameConfig.waves[gameState.currentWave];
        gameState.currentWave++;

        _enemySpawnManager.SpawnWave(currentWave);
    }

    public void SetAutoWave(bool value)
    {
        // TODO
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
        this.RequireComponent<TowerSpawnManager>();

        WorldCell processorCell = mapManager.GetCellAt(gameConfig.mapConfig.processorPosition);

        if (!_towerSpawnManager.SpawnTower(gameConfig.processor, processorCell, out long id, force: true, register: false))
        {
            throw new InvalidOperationException("could not spawn processor");
        }

        gameState.processorState = new ProcessorState { id = id, cell = processorCell };
    }

    #region Needed components

    private TowerSpawnManager _towerSpawnManager;
    private EnemySpawnManager _enemySpawnManager;
    private TowerSpawnPreviewManager _towerSpawnPreviewManager;










    TowerSpawnManager INeedsComponent<TowerSpawnManager>.Component {
        get => _towerSpawnManager;
        set => _towerSpawnManager = value;
    }










    EnemySpawnManager INeedsComponent<EnemySpawnManager>.GetInstance()
    {
        return EnemySpawnManager.Instance;
    }










    EnemySpawnManager INeedsComponent<EnemySpawnManager>.Component {
        get => _enemySpawnManager;
        set => _enemySpawnManager = value;
    }










    TowerSpawnPreviewManager INeedsComponent<TowerSpawnPreviewManager>.GetInstance()
    {
        return TowerSpawnPreviewManager.Instance;
    }










    TowerSpawnPreviewManager INeedsComponent<TowerSpawnPreviewManager>.Component {
        get => _towerSpawnPreviewManager;
        set => _towerSpawnPreviewManager = value;
    }










    TowerSpawnManager INeedsComponent<TowerSpawnManager>.GetInstance()
    {
        return TowerSpawnManager.Instance;
    }

    #endregion
}
