using System;
using System.Linq;
using GameEngine.Map;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using Utils.Extensions;

namespace Managers.Map
{
    public class MapManager : MonoBehaviour
    {
        [NonSerialized]
        public MapConfig MapConfig;
        public GameMap GameMap;
        
        private Tilemap _tilemap;

        void Start()
        {
            Assert.IsNotNull(MapConfig);
            Assert.IsNotNull(MapConfig.gameObject);

            Vector2Int mapHalfSize = MapConfig.mapSize / 2;
            Transform map = Instantiate(MapConfig.gameObject,
                                        new Vector3(-mapHalfSize.x - MapConfig.bottomLeftCorner.x, -mapHalfSize.y - MapConfig.bottomLeftCorner.y, 1),
                                        Quaternion.identity,
                                        transform);

            map.TryGetComponentInSelfOrChildren(out _tilemap);
            Assert.IsNotNull(_tilemap);

            Debug.Log($"Instantiated map with path: {string.Join(", ", MapConfig.path.Select(p => p.ToString()))}");

            GameMap = new GameMap(MapConfig);
        }

        public WorldCell GetCellFromWorldPosition(Vector2 position)
        {
            Cell result = GameMap.GetCellFromLocalWorldPosition(position - (Vector2)_tilemap.transform.position);
            return OfCell(result);
        }

        public WorldCell GetCellAt(Vector2Int cell)
        {
            Cell result = GameMap.GetCellAt(cell);
            return OfCell(result);
        }

        private WorldCell OfCell(Cell cell)
        {
            Vector2 localWorldPosition = GameMap.GetLocalWorldPosition(cell);
            return cell.WithWorldPosition(localWorldPosition + (Vector2)_tilemap.transform.position);
        }
    }
}
