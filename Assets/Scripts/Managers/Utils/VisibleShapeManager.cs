using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using GameEngine.Shapes;
using UnityEngine;
using Utils.CustomComponents;
using Utils.Extensions;

namespace Managers.Utils
{
    public class VisibleShapeManager : MyMonoBehaviour
    {
        public static VisibleShapeManager Instance { get; private set; }

        public SpriteRenderer cellPrefab;

        private Transform _root;
        private List<SpriteRenderer> _previewCells = new();

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            RequireGameManager();

            if (!cellPrefab)
            {
                throw new InvalidOperationException("preview cell not found");
            }

            _root = new GameObject("Root").transform;
            _root.SetParent(transform);
        }

        public void Show(Shape shape, Vector2 position, Color? color = null)
        {
            _root.gameObject.SetActive(true);

            IEnumerable<Vector2Int> cells = shape != null ? shape.EvaluateAt(Vector2Int.zero) : new[] { Vector2Int.zero };

            WorldCell offset = GameManager.Map.GetCellAt(Vector2Int.zero);
            WorldCell[] worldCells = cells.Select(GameManager.Map.GetCellAt).ToArray();

            for (int i = _previewCells.Count; i < worldCells.Length; i++)
            {
                SpriteRenderer newCell = Instantiate(cellPrefab, _root);
                _previewCells.Add(newCell);
            }

            for (int i = 0; i < _previewCells.Count; i++)
            {
                _previewCells[i].gameObject.SetActive(i < worldCells.Length);
            }

            for (int i = 0; i < worldCells.Length; i++)
            {
                _previewCells[i].transform.localPosition = worldCells[i].worldPosition - offset.worldPosition;
            }

            SetPosition(position);

            if (color.HasValue)
            {
                SetColor(color.Value);
            }
        }

        public void Hide()
        {
            _root.gameObject.SetActive(false);
        }

        public void SetColor(Color color)
        {
            foreach (SpriteRenderer spriteRenderer in _previewCells)
            {
                spriteRenderer.color = color;
            }
        }

        public void SetPosition(Vector2 position, bool aboveEntities = false)
        {
            _root.position = position.WithDepth( aboveEntities ? GameConstants.UiLayer + 0.1f : GameConstants.EntityLayer + 0.1f);
        }
    }
}
