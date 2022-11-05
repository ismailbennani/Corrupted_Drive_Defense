using Controllers;
using GameEngine.Shapes;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig : ScriptableObject
    {
        public string towerName;

        [Header("Caracteristics")]
        public Shape shape;

        public bool canRotate;

        public int cost;

        public TowerDescription naked;

        [Header("Upgrades")]
        public TowerUpgrade[] upgradePath1;

        public TowerUpgrade[] upgradePath2;

        public TowerUpgrade[][] UpgradePaths => new[] { upgradePath1, upgradePath2 };

        [Space(10)]
        public TowerController prefab;

        public Sprite sprite;
    }
}
