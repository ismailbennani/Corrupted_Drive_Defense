using Controllers;
using UnityEngine;

namespace GameEngine.Processor
{
    [CreateAssetMenu(menuName = "Objects/Processor config")]
    public class ProcessorConfig : ScriptableObject
    {
        [Tooltip("Size of the processor on the board")]
        public Vector2Int size;
        [Tooltip("Offset of the processor on the board, relative to the bottom left corner")]
        public Vector2Int offset;
        
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
