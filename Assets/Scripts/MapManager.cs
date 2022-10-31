using System;
using System.Linq;
using GameEngine.Map;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Extensions;

public class MapManager : MonoBehaviour
{
    public MapConfig mapConfig;
    public GameMap GameMap;
    public Tilemap tilemap;
    public Transform towersRoot;

    public Vector2 MapOffset => tilemap.transform.position;

    public void Initialize()
    {
        if (!mapConfig || !mapConfig.gameObject)
        {
            throw new NullReferenceException("map prefab not set");
        }

        Vector2Int mapHalfSize = mapConfig.mapSize / 2;
        Transform map = Instantiate(mapConfig.gameObject,
                                    new Vector3(-mapHalfSize.x - mapConfig.bottomLeftCorner.x, -mapHalfSize.y - mapConfig.bottomLeftCorner.y, 1),
                                    Quaternion.identity,
                                    transform);

        if (!map.TryGetComponentInSelfOrChildren(out tilemap))
        {
            throw new InvalidOperationException("could not find grid in map prefab");
        }

        Debug.Log($"Instantiated map with path: {string.Join(", ", mapConfig.path.Select(p => p.ToString()))}");

        towersRoot = new GameObject("TowersRoot").transform;
        towersRoot.SetParent(transform);
        towersRoot.position = Vector2.zero.WithDepth(GameConstants.TowerLayer);

        GameMap = new GameMap(mapConfig);
    }

    public Vector2 GetWorldPositionOfCell(Vector2Int cell)
    {
        return GetCellAt(cell).worldPosition;
    }

    public WorldCell GetCellFromWorldPosition(Vector2 position)
    {
        Cell result = GameMap.GetCellFromLocalWorldPosition(position - (Vector2)tilemap.transform.position);
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
        return cell.WithWorldPosition(localWorldPosition + (Vector2)tilemap.transform.position);
    }
}
