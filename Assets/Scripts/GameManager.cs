using System;
using System.Collections;
using GameComponents;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using GameEngine.Waves;
using UnityEngine;
using Utils.Extensions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameConfig gameConfig;

    [Header("Created on start")]
    public GameState gameState;
    
    public MapManager mapManager;
    private TowerSpawnPreviewManager _towerSpawnPreviewManager;
    private TowerSpawnManager _towerSpawnManager;
    private EnemySpawnManager _enemySpawnManager;
    private SelectedTowerManager _selectedTowerManager;

    void Awake()
    {
        Instance = this;

        if (!gameConfig)
        {
            throw new InvalidOperationException("game config not set");
        }

        gameState = new GameState();
    }

    IEnumerator Start()
    {
        this.RemoveAllChildren();

        yield return null;
        
        SpawnMap();

        yield return null;
        
        SpawnTowerManagers();
        SpawnEnemyManagers();
        SpawnProcessor();
    }

    public void StartSpawning(TowerConfig tower)
    {
        _towerSpawnPreviewManager.StartPreview(tower);
    }

    public void SpawnTower(TowerConfig tower, WorldCell cell)
    {
        _towerSpawnManager.SpawnTower(tower, cell);
    }

    public void StartWave()
    {
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
        _selectedTowerManager.Select(state);
    }

    public void UnselectTower()
    {
        _selectedTowerManager.Unselect();
    }

    private void SpawnMap()
    {
        if (!gameConfig.mapConfig)
        {
            throw new InvalidOperationException("map config not set");
        }

        Transform map = new GameObject("MapManager", typeof(MapManager)).transform;
        map.SetParent(transform);
        mapManager = map.GetComponent<MapManager>();
        mapManager.mapConfig = gameConfig.mapConfig;
    }

    private void SpawnTowerManagers()
    {
        Transform towersRoot = new GameObject("TowersRoot", typeof(TowerSpawnManager), typeof(SelectedTowerManager), typeof(TowerSpawnPreviewManager))
            .transform;
        towersRoot.SetParent(transform);
        towersRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

        _towerSpawnPreviewManager = towersRoot.GetComponent<TowerSpawnPreviewManager>();
        _selectedTowerManager = towersRoot.GetComponent<SelectedTowerManager>();

        _towerSpawnManager = towersRoot.GetComponent<TowerSpawnManager>();
        _towerSpawnManager.root = towersRoot;
    }

    private void SpawnEnemyManagers()
    {
        Transform enemiesRoot = new GameObject("EnemiesRoot", typeof(EnemySpawnManager)).transform;
        enemiesRoot.SetParent(transform);
        enemiesRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

        _enemySpawnManager = enemiesRoot.GetComponent<EnemySpawnManager>();
        _enemySpawnManager.root = enemiesRoot;
    }

    private void SpawnProcessor()
    {
        WorldCell processorCell = mapManager.GetCellAt(gameConfig.mapConfig.processorPosition);

        ProcessorConfig processorConfig = gameConfig.processor;

        ProcessorController processor = Instantiate(processorConfig.prefab, Vector3.zero, Quaternion.identity, mapManager.towersRoot);
        processor.transform.localPosition = processorCell.worldPosition.WithDepth(GameConstants.EntityLayer);

        gameState.processorState = new ProcessorState(processorCell, processorConfig);
    }
}
