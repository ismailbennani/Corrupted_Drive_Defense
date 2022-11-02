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
        private Transform _root;
        
        private void Start()
        {
            _root = new GameObject("SpawnRoot").transform;
            _root.SetParent(transform);
        }

        public bool SpawnTower(TowerConfig tower, WorldCell cell, out TowerState state)
        {
            if (!tower || !tower.prefab)
            {
                throw new InvalidOperationException("tower prefab not set");
            }

            Debug.Log($"Spawn {tower.towerName} at {cell.gridPosition}");

            long id = Uid.Get();

            TowerController newTower = Instantiate(tower.prefab, Vector3.zero, Quaternion.identity, _root);
            newTower.transform.localPosition = cell.worldPosition.WithDepth(GameConstants.EntityLayer);
            newTower.id = id;

            state = new TowerState(id, cell.gridPosition, tower);
            return true;
        }
    }
}