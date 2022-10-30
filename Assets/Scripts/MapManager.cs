using System;
using System.Linq;
using GameEngine.Map;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Extensions;

public class MapManager : MonoBehaviour
{
    public MapConfig mapConfig;

    private GameMap _map;
    private Tilemap _tilemap;

    public void Initialize()
    {
        if (!mapConfig || !mapConfig.gameObject)
        {
            throw new NullReferenceException("map prefab not set");
        }

        Vector2Int mapHalfSize = mapConfig.mapSize / 2;
        Transform map = Instantiate(mapConfig.gameObject,
                                    new Vector3(-mapHalfSize.x - mapConfig.topLeftCorner.x, mapHalfSize.y - mapConfig.topLeftCorner.y - 1, 1),
                                    Quaternion.identity,
                                    transform);
        
        if (!map.TryGetComponentInSelfOrChildren(out _tilemap))
        {
            throw new InvalidOperationException("could not find grid in map prefab");
        }

        Debug.Log($"Instantiated map with path: {string.Join(", ", mapConfig.path.Select(p => p.ToString()))}");

        _map = new GameMap(mapConfig);
    }

    /// <summary>
    /// Return the world position of the center of the given cell 
    /// </summary>
    public Vector2 GridCellToWorldPosition(Vector2Int cell)
    {
        return (Vector2)_tilemap.transform.position + _map.GetGridCellPosition(cell);
    }
}
