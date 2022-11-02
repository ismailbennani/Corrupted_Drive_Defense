using GameEngine.Towers;
using UnityEngine;

namespace Managers.Tower.Extensions
{
    public static class TowerSpawnerApiExtensions
    {
        public static bool TrySpawnTower(this TowerSpawnerApi @this, TowerConfig tower, Vector2Int cell, bool rotated)
        {
            return @this.TrySpawnTower(tower, cell, rotated, out _);
        }
    }
}
