using GameEngine.Towers;
using UnityEngine;

namespace Managers.Tower
{
    public static class TowerSpawnerApiExtensions
    {
        public static bool TrySpawnTower(this TowerSpawnerApi @this, TowerConfig tower, Vector2Int cell)
        {
            return @this.TrySpawnTower(tower, cell, out _);
        }
    }
}
