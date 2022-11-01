using System;
using System.Collections;
using Controllers;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using GameEngine.Waves;
using Managers.Tower;
using UnityEngine;
using Utils.Extensions;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameConfig gameConfig;

        [Header("Created on start")]
        public GameState gameState;

        public MapApi Map { get; private set; }

        private MapManager _mapManager;
        private Transform _towersRoot;
        private TowerSpawnPreviewManager _towerSpawnPreviewManager;
        private TowerSpawnManager _towerSpawnManager;
        private SelectedTowerManager _selectedTowerManager;
        private Transform _enemiesRoot;
        private EnemySpawnManager _enemySpawnManager;

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
            Map = new MapApi(_mapManager);

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
            _mapManager = map.GetComponent<MapManager>();
            _mapManager.mapConfig = gameConfig.mapConfig;
        }

        private void SpawnTowerManagers()
        {
            _towersRoot = new GameObject("TowersRoot", typeof(TowerSpawnManager), typeof(SelectedTowerManager), typeof(TowerSpawnPreviewManager))
                .transform;
            _towersRoot.SetParent(transform);
            _towersRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

            _towerSpawnPreviewManager = _towersRoot.GetComponent<TowerSpawnPreviewManager>();
            _selectedTowerManager = _towersRoot.GetComponent<SelectedTowerManager>();

            _towerSpawnManager = _towersRoot.GetComponent<TowerSpawnManager>();
            _towerSpawnManager.root = _towersRoot;
        }

        private void SpawnEnemyManagers()
        {
            _enemiesRoot = new GameObject("EnemiesRoot", typeof(EnemySpawnManager)).transform;
            _enemiesRoot.SetParent(transform);
            _enemiesRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

            _enemySpawnManager = _enemiesRoot.GetComponent<EnemySpawnManager>();
            _enemySpawnManager.root = _enemiesRoot;
        }

        private void SpawnProcessor()
        {
            WorldCell processorCell = Map.GetCellAt(gameConfig.mapConfig.processorPosition);

            ProcessorConfig processorConfig = gameConfig.processor;

            ProcessorController processor = Instantiate(processorConfig.prefab, Vector3.zero, Quaternion.identity, _towersRoot);
            processor.transform.localPosition = processorCell.worldPosition.WithDepth(GameConstants.EntityLayer);

            gameState.processorState = new ProcessorState(processorCell, processorConfig);
        }
    }
}