using System;
using System.Collections;
using Controllers;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using GameEngine.Waves;
using Managers.Enemy;
using Managers.Map;
using Managers.Tower;
using Managers.Utils;
using UnityEngine;
using Utils.Extensions;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameConfig gameConfig;

        public GameStateApi GameState { get; private set; }
        public MapApi Map { get; private set; }
        public VisibleShapeApi VisibleShape { get; private set; }
        public TowerSpawnerApi TowerSpawner { get; private set; }
        public SelectedTowerApi SelectedTower { get; private set; }
        public EnemySpawnApi EnemySpawn { get; private set; }

        private Transform _towersRoot;
        private TowerSpawnPreviewManager _towerSpawnPreviewManager;
        private Transform _enemiesRoot;

        void Awake()
        {
            Instance = this;

            if (!gameConfig)
            {
                throw new InvalidOperationException("game config not set");
            }
        }

        IEnumerator Start()
        {
            this.RemoveAllChildren();

            yield return null;

            SpawnMap();

            yield return null;
            
            GameState = new GameStateApi(new GameState(), Map);

            yield return null;
        
            SpawnUtils();
            
            yield return null;
            
            SpawnTowerManagers();
            SpawnEnemyManagers();
            SpawnProcessor();
        }

        public void StartSpawning(TowerConfig tower)
        {
            _towerSpawnPreviewManager.StartPreview(tower);
        }

        public void StartWave()
        {
            WaveConfig currentWave = gameConfig.waves[GameState.GetCurrentWave()];
            GameState.IncrementWave();
            

            EnemySpawn.SpawnWave(currentWave);
        }

        public void SetAutoWave(bool auto)
        {
            EnemySpawn.SetAutoWave(auto);
        }

        public void SelectTower(TowerState state)
        {
            SelectedTower.Select(state);
        }

        public void UnselectTower()
        {
            SelectedTower.Clear();
        }

        private void SpawnMap()
        {
            if (!gameConfig.mapConfig)
            {
                throw new InvalidOperationException("map config not set");
            }

            Transform map = new GameObject("MapManager", typeof(MapManager)).transform;
            map.SetParent(transform);
            
            MapManager mapManager = map.GetComponent<MapManager>();
            mapManager.mapConfig = gameConfig.mapConfig;
            
            Map = new MapApi(mapManager);
        }

        private void SpawnUtils()
        {
            Transform utilsRoot = new GameObject("Utils", typeof(VisibleShapeManager)).transform;
            utilsRoot.SetParent(transform);
            
            VisibleShapeManager visibleShapeManager = utilsRoot.GetComponent<VisibleShapeManager>();
            visibleShapeManager.Map = Map;
            visibleShapeManager.cellPrefab = gameConfig.cellPrefab;
            VisibleShape = new VisibleShapeApi(visibleShapeManager);
        }

        private void SpawnTowerManagers()
        {
            _towersRoot = new GameObject("TowersRoot", typeof(TowerSpawnerManager), typeof(SelectedTowerManager), typeof(TowerSpawnPreviewManager))
                .transform;
            _towersRoot.SetParent(transform);
            _towersRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

            TowerSpawnerManager towerSpawnerManager = _towersRoot.GetComponent<TowerSpawnerManager>();
            towerSpawnerManager.root = _towersRoot;
            TowerSpawner = new TowerSpawnerApi(towerSpawnerManager, GameState);
            
            _towerSpawnPreviewManager = _towersRoot.GetComponent<TowerSpawnPreviewManager>();
            _towerSpawnPreviewManager.GameConfig = gameConfig;
            _towerSpawnPreviewManager.TowerSpawner = TowerSpawner;
            _towerSpawnPreviewManager.VisibleShape = VisibleShape;
            
            SelectedTowerManager selectedTowerManager = _towersRoot.GetComponent<SelectedTowerManager>();
            selectedTowerManager.VisibleShape = VisibleShape;
            SelectedTower = new SelectedTowerApi(selectedTowerManager);
        }

        private void SpawnEnemyManagers()
        {
            _enemiesRoot = new GameObject("EnemiesRoot", typeof(EnemySpawnManager)).transform;
            _enemiesRoot.SetParent(transform);
            _enemiesRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

            EnemySpawnManager enemySpawnManager = _enemiesRoot.GetComponent<EnemySpawnManager>();
            enemySpawnManager.root = _enemiesRoot;
            EnemySpawn = new EnemySpawnApi(enemySpawnManager);
        }

        private void SpawnProcessor()
        {
            WorldCell processorCell = Map.GetCellAt(gameConfig.mapConfig.processorPosition);

            ProcessorConfig processorConfig = gameConfig.processor;

            ProcessorController processor = Instantiate(processorConfig.prefab, Vector3.zero, Quaternion.identity, _towersRoot);
            processor.transform.localPosition = processorCell.worldPosition.WithDepth(GameConstants.EntityLayer);

            GameState.SetProcessorState(new ProcessorState(processorCell, processorConfig));
        }
    }
}