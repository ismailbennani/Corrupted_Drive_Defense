using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Map;
using GameEngine.Shapes;
using Managers.Map;
using UnityEngine;
using UnityEngine.Assertions;
using Utils.Extensions;

namespace Managers.Utils
{
    public class VisibleShapeManager : MonoBehaviour
    {
        public MapApi Map;

        [NonSerialized]
        public SpriteRenderer CellPrefab;

        private Transform _root;
        private readonly List<SpriteRenderer> _previewCells = new();
        private IShape _lastShape;
        private bool _rotated;

        private void Start()
        {
            Assert.IsNotNull(Map);
            Assert.IsNotNull(CellPrefab);

            _root = new GameObject("VisibleShapeRoot").transform;
            _root.position = Vector2.zero.WithDepth(GameConstants.UiLayer + 0.1f);
            _root.SetParent(transform);
        }

        public void Show(IShape shape, bool rotated, Color? color = null, bool? aboveEntities = null, params Vector2Int[] positions)
        {
            _lastShape = shape;
            _rotated = rotated;

            _root.gameObject.SetActive(true);

            if (aboveEntities.HasValue)
            {
                _root.position = Vector2.zero.WithDepth(aboveEntities.Value ? GameConstants.UiLayer + 0.1f : GameConstants.EntityLayer + 0.1f);
            }

            IEnumerable<Vector2Int> cells = shape != null ? shape.EvaluateAt(positions, rotated) : positions;

            ShowCells(cells);

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

        public void SetPositions(params Vector2Int[] positions)
        {
            Show(_lastShape, _rotated, null, null, positions);
        }

        private void ShowCells(IEnumerable<Vector2Int> cells)
        {
            WorldCell[] worldCells = cells.Select(Map.GetCellAt).ToArray();

            for (int i = _previewCells.Count; i < worldCells.Length; i++)
            {
                SpriteRenderer newCell = Instantiate(CellPrefab, _root);
                _previewCells.Add(newCell);
            }

            for (int i = 0; i < _previewCells.Count; i++)
            {
                _previewCells[i].gameObject.SetActive(i < worldCells.Length);
            }

            for (int i = 0; i < worldCells.Length; i++)
            {
                _previewCells[i].transform.localPosition = worldCells[i].worldPosition;
            }
        }
    }
}
