using System;
using Controllers;
using GameEngine.Map;
using GameEngine.Towers;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace Managers.Tower
{
    public class TowerSpawnerManager : MonoBehaviour
    {
        public Transform root;

        public bool SpawnTower(TowerConfig tower, WorldCell cell, out TowerState state, bool force = false)
        {
            if (!tower || !tower.prefab)
            {
                throw new InvalidOperationException("tower prefab not set");
            }

            if (!root)
            {
                throw new InvalidOperationException("towers root not set");
            }

            if (!force)
            {
                if (cell.type != CellType.Free)
                {
                    state = null;
                    return false;
                }
            }

            Debug.Log($"Spawn {tower.towerName} at {cell.gridPosition}");

            long id = Uid.Get();

            TowerController newTower = Instantiate(tower.prefab, Vector3.zero, Quaternion.identity, root);
            newTower.transform.localPosition = cell.worldPosition.WithDepth(GameConstants.EntityLayer);
            newTower.id = id;

            state = new TowerState(id, cell, tower);
            return true;
        }
    }
}