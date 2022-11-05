using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using GameEngine.Map;
using GameEngine.Shapes;
using GameEngine.Towers;
using Managers.Tower.Extensions;
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
        public UnityEvent onStopPreview = new();

        [NonSerialized]
        public GameConfig GameConfig;

        public TowerSpawnerApi TowerSpawner;
        public VisibleShapeApi VisibleShape;

        private Transform _root;
        private TowerConfig _tower;
        private Transform _previewTower;
        private bool _rotated;

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

            bool canSpawn = TowerSpawner.CanSpawnTowerAt(_tower, cell.gridPosition, _rotated);
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
            _rotated = _tower.canRotate && _rotated;
            Debug.Log($"Start preview of {tower.towerName}");

            _root.gameObject.SetActive(true);

            if (_previewTower)
            {
                Destroy(_previewTower.gameObject);
            }

            _previewTower = Instantiate(tower.prefab, _root).transform;

            SetPreviewTowerRotation();
            ShowVisibleShape();
        }

        public void ToggleRotated()
        {
            _rotated = _tower.canRotate && !_rotated;

            SetPreviewTowerRotation();
            ShowVisibleShape();
        }

        private void SetPreviewTowerRotation()
        {
            if (_previewTower)
            {
                _previewTower.rotation = _rotated ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
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

            onStopPreview.Invoke();
        }

        private void SpawnAt(WorldCell cell)
        {
            if (TowerSpawner.TrySpawnTower(_tower, cell.gridPosition, _rotated))
            {
                StopPreview();
            }
        }

        private IEnumerable<Vector2Int> GetCells(TowerConfig tower)
        {
            WorldCell cell = Mouse.GetTargetCell();
            return tower.shape.EvaluateAt(Vector2Int.zero, _rotated).Select(c => c + cell.gridPosition);
        }

        private void ShowVisibleShape()
        {
            switch (_tower.naked.targetType)
            {
                case TargetType.None:
                    VisibleShape.Show(null, GetCells(_tower), _rotated, aboveEntities: true);
                    break;
                case TargetType.Single:
                case TargetType.AreaAtTarget:
                    VisibleShape.Show(_tower.naked.range, GetCells(_tower), _rotated, aboveEntities: true);
                    break;
                case TargetType.AreaAtSelf:
                    VisibleShape.Show(_tower.naked.targetShape, GetCells(_tower), _rotated, aboveEntities: true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
