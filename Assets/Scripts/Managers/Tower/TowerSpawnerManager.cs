using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using GameEngine.Map;
using GameEngine.Towers;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace Managers.Tower
{
    public class TowerSpawnerManager : MonoBehaviour
    {
        private Transform _root;
        private readonly List<TowerController> _towers = new();
        
        private void Start()
        {
            _root = new GameObject("SpawnRoot").transform;
            _root.SetParent(transform);
        }

        public void SpawnTower(TowerConfig tower, WorldCell cell, out TowerState state)
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
            _towers.Add(newTower);

            state = new TowerState(id, cell.gridPosition, tower);
        }

        public void DestroyTower(long id)
        {
            TowerController controller = _towers.SingleOrDefault(c => c.id == id);
            if (controller == null)
            {
                throw new InvalidOperationException($"Could not find tower controller with id {id}");
            }

            _towers.Remove(controller);
            
            Destroy(controller.GameObject());
        }
    }
}