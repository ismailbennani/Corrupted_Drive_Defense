using Controllers;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Processor config")]
    public class ProcessorConfig : ScriptableObject
    {
        [Header("Health")]
        public int maxHealth = 100;

        [Header("Charge")]
        [Tooltip("Charge produced per second")]
        public float frequency = 10;
        public float maxCharge = 1000;
        
        [Space(10)]
        public ProcessorController prefab;
        public Sprite sprite;
    }
}
