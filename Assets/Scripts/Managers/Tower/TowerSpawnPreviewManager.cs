using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Utils;
using Utils.Extensions;

namespace Managers.Tower
{
    public class TowerSpawnPreviewManager : MonoBehaviour
    {
        public UnityEvent OnStopPreview = new UnityEvent();
        
        [NonSerialized]
        public GameConfig GameConfig;
        public TowerSpawnerApi TowerSpawner;
        public VisibleShapeApi VisibleShape;
        
        private Transform _root;
        private TowerConfig _tower;
        private Transform _previewTower;

        void Start()
        {
            Assert.IsNotNull(GameConfig);
            Assert.IsNotNull(VisibleShape);
            
            _root = new GameObject("SpawnPreviewRoot").transform;
            _root.SetParent(transform);   
        
            StopPreview();
        }

        void Update()
        {
            if (!_tower)
            {
                return;
            }

            WorldCell cell = Mouse.GetTargetCell();
            _root.position = cell.worldPosition.WithDepth(GameConstants.UiLayer);
            VisibleShape.SetPositions(GetCells(_tower));
            
            bool canSpawn = TowerSpawner.CanSpawnTowerAt(_tower, cell.gridPosition);
            VisibleShape.SetColor(canSpawn ? GameConfig.shapePreviewOkColor : GameConfig.shapePreviewErrorColor);

            if (canSpawn && Input.GetMouseButtonUp(0))
            {
                SpawnAt(cell);
            }

            if (Input.GetMouseButtonUp(1))
            {
                StopPreview();
            }
        }

        public void StartPreview(TowerConfig tower)
        {
            _tower = tower;
            Debug.Log($"Start preview of {tower.towerName}");

            _root.gameObject.SetActive(true);

            if (_previewTower)
            {
                Destroy(_previewTower.gameObject);
            }

            _previewTower = Instantiate(tower.prefab, _root).transform;

            switch (tower.targetType)
            {
                case TargetType.Single:
                case TargetType.AreaAtTarget:
                    VisibleShape.Show(tower.range, GetCells(tower), aboveEntities: true);
                    break;
                case TargetType.AreaAtSelf:
                    VisibleShape.Show(tower.targetShape, GetCells(tower), aboveEntities: true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StopPreview()
        {
            if (_tower)
            {
                Debug.Log($"Stop preview of {_tower.towerName}");
            }

            _tower = null;
            _root.gameObject.SetActive(false);
            VisibleShape.Hide();
            
            OnStopPreview.Invoke();
        }

        private void SpawnAt(WorldCell cell)
        {
            if (TowerSpawner.TrySpawnTower(_tower, cell.gridPosition))
            {
                StopPreview();
            }
        }

        private IEnumerable<Vector2Int> GetCells(TowerConfig tower)
        {
            WorldCell cell = Mouse.GetTargetCell();
            return tower.shape.EvaluateAt(Vector2Int.zero).Select(c => c + cell.gridPosition);
        }
    }
}
