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

        void Start()
        {
            Assert.IsNotNull(Map);
            Assert.IsNotNull(CellPrefab);

            _root = new GameObject("VisibleShapeRoot").transform;
            _root.SetParent(transform);
        }

        public void Show(IShape shape, Vector2Int position, Color? color = null)
        {
            _root.gameObject.SetActive(true);

            IEnumerable<Vector2Int> cells = shape != null ? shape.EvaluateAt(Vector2Int.zero) : new[] { Vector2Int.zero };

            WorldCell offset = Map.GetCellAt(Vector2Int.zero);
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

        public void SetPosition(Vector2Int position, bool aboveEntities = false)
        {
            _root.position = Map.GetCellAt(position)
                .worldPosition.WithDepth(aboveEntities ? GameConstants.UiLayer + 0.1f : GameConstants.EntityLayer + 0.1f);
        }
    }
}
