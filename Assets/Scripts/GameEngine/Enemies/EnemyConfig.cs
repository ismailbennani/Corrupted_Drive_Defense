using GameComponents;
using UnityEngine;

namespace GameEngine.Enemies
{
    [CreateAssetMenu(menuName = "Objects/Enemy config")]
    public class EnemyConfig: ScriptableObject
    {
        public string enemyName;
        public EnemyController prefab;
        public Sprite sprite;


        [Space(10)]
        public EnemyConfig child;

        [Header("Characteristics")]
        public int hp;

        [Tooltip("in cell/sec")]
        public float speed;
    }
}
