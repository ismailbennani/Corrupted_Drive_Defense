using System;
using GameEngine.Map;
using GameEngine.Tower;
using UnityEngine;
using Utils.Extensions;
using Utils.Interfaces;

public class TowerSpawnManager : MonoBehaviour, INeedsGameManager
{
    public static TowerSpawnManager Instance { get; private set; }
    public GameManager GameManager { get; set; }

    public Transform root;
    
    void Awake()
    {
        Instance = this;
    }

    public bool SpawnTower(TowerConfig tower, WorldCell cell, bool force = false)
    {
        if (!tower || !tower.prefab)
        {
            throw new InvalidOperationException("tower prefab not set");
        }
        
        this.RequireGameManager();

        root = GameManager.mapManager.towersRoot;

        if (!force)
        {
            if (cell.type != CellType.Free)
            {
                return false;
            }
        }

        Debug.Log($"Spawn {tower.name} at {cell.gridPosition}");

        Transform newTower = Instantiate(tower.prefab, Vector3.zero, Quaternion.identity, root);
        newTower.localPosition = cell.worldPosition.WithDepth(GameConstants.TowerLayer);

        return true;
    }
}
