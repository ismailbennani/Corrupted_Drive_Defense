using GameEngine.Map;
using GameEngine.Towers;
using Managers.Utils;
using UnityEngine;
using Utils;
using Utils.CustomComponents;
using Utils.Extensions;

namespace Managers.Tower
{
    public class TowerSpawnPreviewManager : MyMonoBehaviour
    {
        private Transform _root;
        private TowerConfig _tower;
        private Transform _previewTower;

        void Start()
        {
            RequireGameManager();
     
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
            GameManager.VisibleShapeManager.SetPosition(cell.gridPosition, true);
            GameManager.VisibleShapeManager.SetColor(cell.type == CellType.Free ? GameManager.gameConfig.shapePreviewOkColor : GameManager.gameConfig.shapePreviewErrorColor);

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
            GameManager.VisibleShapeManager.Show(tower.targetArea, Vector2Int.zero);
        }

        public void StopPreview()
        {
            if (_tower)
            {
                Debug.Log($"Stop preview of {_tower.towerName}");
            }

            _tower = null;
            _root.gameObject.SetActive(false);
            GameManager.VisibleShapeManager.Hide();
        }

        public void SpawnAt(WorldCell cell)
        {
            if (cell.type != CellType.Free)
            {
                Debug.LogWarning($"Cannot build tower at {cell.gridPosition} because it is not free: {cell.type}");
                return;
            }

            GameManager.SpawnTower(_tower, cell);
            StopPreview();
        }
    }
}
