using Controllers;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Processor config")]
    public class ProcessorConfig : ScriptableObject
    {
        [Header("Health")]
        public int maxHealth = 100;

        [Header("Ticks")]
        [Tooltip("Ticks per seconds that are replenished")]
        public float frequency = 10;
        public float maxTicks = 1000;
        
        [Space(10)]
        public ProcessorController prefab;
        public Sprite sprite;
    }
}
