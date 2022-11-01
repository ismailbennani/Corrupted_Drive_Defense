using System;
using System.Collections;
using Controllers;
using GameEngine;
using GameEngine.Map;
using GameEngine.Processor;
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

        public bool Ready { get; private set; }
        public GameStateApi GameState { get; private set; }
        public MapApi Map { get; private set; }
        public GameSpeedApi GameSpeed { get; private set; }
        public VisibleShapeApi VisibleShape { get; private set; }
        public MouseInputApi MouseInput { get; private set; }
        public TowerSpawnerApi TowerSpawner { get; private set; }
        public TowerSpawnPreviewApi TowerSpawnPreview { get; private set; }
        public SelectedEntityApi SelectedEntity { get; private set; }
        public EnemySpawnApi EnemySpawn { get; private set; }
        public EnemyDamageApi EnemyDamage { get; private set; }
        public EnemyWaveApi EnemyWave { get; private set; }

        private Transform _towersRoot;
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

            SpawnEnemyManagers();
            SpawnTowerManagers();
            SpawnProcessor();

            yield return null;

            SpawnOtherUtils();

            Ready = true;
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
            mapManager.MapConfig = gameConfig.mapConfig;

            Map = new MapApi(mapManager);
        }

        private void SpawnUtils()
        {
            GameSpeed = new GameSpeedApi();
            
            Transform utilsRoot = new GameObject("Utils", typeof(VisibleShapeManager)).transform;
            utilsRoot.SetParent(transform);

            VisibleShapeManager visibleShapeManager = utilsRoot.GetComponent<VisibleShapeManager>();
            visibleShapeManager.Map = Map;
            visibleShapeManager.CellPrefab = gameConfig.cellPrefab;
            VisibleShape = new VisibleShapeApi(visibleShapeManager);
        }

        private void SpawnEnemyManagers()
        {
            _enemiesRoot = new GameObject("Enemies", typeof(EnemySpawnManager), typeof(EnemyWaveManager)).transform;
            _enemiesRoot.SetParent(transform);
            _enemiesRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

            EnemySpawnManager enemySpawnManager = _enemiesRoot.GetComponent<EnemySpawnManager>();
            enemySpawnManager.GameState = GameState;
            enemySpawnManager.Map = Map;
            EnemySpawn = new EnemySpawnApi(enemySpawnManager);

            EnemyWaveManager enemyWaveManager = _enemiesRoot.GetComponent<EnemyWaveManager>();
            enemyWaveManager.Spawn = gameConfig.mapConfig.path[0];
            enemyWaveManager.EnemySpawn = EnemySpawn;
            EnemyWave = new EnemyWaveApi(gameConfig, GameState, enemyWaveManager);

            EnemyDamage = new EnemyDamageApi(GameState, EnemySpawn);
        }

        private void SpawnTowerManagers()
        {
            _towersRoot = new GameObject("Towers", typeof(TowerSpawnerManager), typeof(TowerSpawnPreviewManager)).transform;
            _towersRoot.SetParent(transform);
            _towersRoot.position = Vector2.zero.WithDepth(GameConstants.EntityLayer);

            TowerSpawnerManager towerSpawnerManager = _towersRoot.GetComponent<TowerSpawnerManager>();
            TowerSpawner = new TowerSpawnerApi(towerSpawnerManager, GameState, Map);

            TowerSpawnPreviewManager towerSpawnPreviewManager = _towersRoot.GetComponent<TowerSpawnPreviewManager>();
            towerSpawnPreviewManager.GameConfig = gameConfig;
            towerSpawnPreviewManager.TowerSpawner = TowerSpawner;
            towerSpawnPreviewManager.VisibleShape = VisibleShape;
            TowerSpawnPreview = new TowerSpawnPreviewApi(towerSpawnPreviewManager);

            SelectedEntity = new SelectedEntityApi(VisibleShape);
        }

        private void SpawnProcessor()
        {
            WorldCell processorCell = Map.GetCellAt(gameConfig.mapConfig.processorPosition);

            ProcessorConfig processorConfig = gameConfig.processor;

            ProcessorController processor = Instantiate(processorConfig.prefab, Vector3.zero, Quaternion.identity, _towersRoot);
            processor.transform.localPosition = processorCell.worldPosition.WithDepth(GameConstants.EntityLayer);

            GameState.SetProcessorState(new ProcessorState(gameConfig.mapConfig.processorPosition, processorConfig));
        }

        private void SpawnOtherUtils()
        {
            MouseInput = new MouseInputApi(GameState, SelectedEntity);
        }

        #region Exposed APIs to inspector
        
        public void StartSpawning(TowerConfig tower)
        {
            TowerSpawnPreview.StartPreview(tower);
        }

        public void StartWave()
        {
            EnemyWave.SpawnNextWave();
        }

        public void SetAutoWave(bool auto)
        {
            EnemyWave.SetAutoWave(auto);
        }

        public void CycleSpeed()
        {
            GameSpeed.CycleSpeed();
        }
        
        #endregion
    }
}
