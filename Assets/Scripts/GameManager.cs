using System;
using System.Linq;
using GameComponents;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using GameEngine.Waves;
using UnityEngine;
using Utils.CustomComponents;
using Utils.Extensions;

public class GameManager
    : MonoBehaviour, INeedsComponent<TowerSpawnManager>, INeedsComponent<TowerSpawnPreviewManager>, INeedsComponent<EnemySpawnManager>,
        INeedsComponent<SelectedTowerManager>
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

    public void SelectTower(TowerState state)
    {
        this.RequireComponent<SelectedTowerManager>();
        _selectedTowerManager.Select(state);
    }
    
    public void UnselectTower()
    {
        this.RequireComponent<SelectedTowerManager>();
        _selectedTowerManager.Unselect();
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

        ProcessorConfig processorConfig = gameConfig.processor;

        ProcessorController processor = Instantiate(processorConfig.prefab, Vector3.zero, Quaternion.identity, mapManager.towersRoot);
        processor.transform.localPosition = processorCell.worldPosition.WithDepth(GameConstants.EntityLayer);

        gameState.processorState = new ProcessorState(processorCell, processorConfig);
    }

    #region Needed components

    private TowerSpawnManager _towerSpawnManager;
    private EnemySpawnManager _enemySpawnManager;
    private TowerSpawnPreviewManager _towerSpawnPreviewManager;
    private SelectedTowerManager _selectedTowerManager;










    TowerSpawnManager INeedsComponent<TowerSpawnManager>.Component {
        get => _towerSpawnManager;
        set => _towerSpawnManager = value;
    }










    SelectedTowerManager INeedsComponent<SelectedTowerManager>.Component {
        get => _selectedTowerManager;
        set => _selectedTowerManager = value;
    }










    public SelectedTowerManager GetInstance()
    {
        return SelectedTowerManager.Instance;
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
