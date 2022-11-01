using System;
using GameEngine;
using GameEngine.Map;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Utils.Extensions;

namespace Managers.Tower
{
    public class TowerSpawnPreviewManager : MonoBehaviour
    {
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
            
            _root = new GameObject("Root").transform;
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
            VisibleShape.SetPosition(cell.gridPosition, true);
            VisibleShape.SetColor(cell.type == CellType.Free ? GameConfig.shapePreviewOkColor : GameConfig.shapePreviewErrorColor);

            if (Input.GetMouseButtonUp(0))
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
            VisibleShape.Show(tower.targetArea, Vector2Int.zero);
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
        }

        private void SpawnAt(WorldCell cell)
        {
            if (TowerSpawner.TrySpawnTower(_tower, cell.gridPosition))
            {
                StopPreview();
            }
        }
    }
}
