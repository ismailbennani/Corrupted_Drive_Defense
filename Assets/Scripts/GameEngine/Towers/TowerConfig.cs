using Controllers;
using GameEngine.Enemies.Effects;
using GameEngine.Shapes;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig: ScriptableObject
    {
        public string towerName;

        [Header("Caracteristics")]
        public Shape shape;
        public bool canRotate;
        public int cost;
        
        [Header("Charge")]
        [Tooltip("Charge consumed from CPU per second")]
        public float frequency;
        public int maxCharge;
        
        [Header("Target")]
        public TargetType targetType;
        [Tooltip("Ignored when targetType is AreaAtSelf ")]
        public TargetShape range;
        [Tooltip("Ignored when targetType is Single ")]
        public TargetShape targetShape;
        
        [Header("Base effect")]
        public int baseDamage;
        public TowerHitEffect towerHitEffect;

        [Space(10)]
        public TowerController prefab;
        public Sprite sprite;
    }
}
