using System;
using GameComponents;
using GameEngine.Map;
using GameEngine.Towers;
using UnityEngine;
using Utils;
using Utils.CustomComponents;
using Utils.Extensions;

public class TowerSpawnManager : MyMonoBehaviour
{
    public static TowerSpawnManager Instance { get; private set; }

    public Transform root;

    void Awake()
    {
        Instance = this;
    }

    public bool SpawnTower(TowerConfig tower, WorldCell cell, out long id, bool force = false, bool register = true)
    {
        if (!tower || !tower.prefab)
        {
            throw new InvalidOperationException("tower prefab not set");
        }

        RequireGameManager();
        
        if (!root)
        {
            throw new InvalidOperationException("towers root not set");
        }

        if (!force)
        {
            if (cell.type != CellType.Free)
            {
                id = -1;
                return false;
            }
        }

        Debug.Log($"Spawn {tower.towerName} at {cell.gridPosition}");

        id = Uid.Get();

        TowerController newTower = Instantiate(tower.prefab, Vector3.zero, Quaternion.identity, root);
        newTower.transform.localPosition = cell.worldPosition.WithDepth(GameConstants.EntityLayer);
        newTower.id = id;

        if (register)
        {
            TowerState newTowerState = new(id, cell, tower);
            GameManager.gameState.towerStates.Add(newTowerState);
        }

        return true;
    }
}

public static class TowerSpawnManagerExtensions
{
    public static bool SpawnTower(this TowerSpawnManager @this, TowerConfig tower, WorldCell cell, bool force = false, bool register = true)
    {
        return @this.SpawnTower(tower, cell, out _, force, register);
    }
}
