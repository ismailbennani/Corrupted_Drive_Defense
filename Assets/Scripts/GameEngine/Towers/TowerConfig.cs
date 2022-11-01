using Controllers;
using GameEngine.Shapes;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig: ScriptableObject
    {
        public string towerName;
        public Shape targetArea;
        
        [Header("Charge")]
        [Tooltip("Charge consumed from CPU per second")]
        public float frequency;
        public int maxCharge;

        [Space(10)]
        public TowerController prefab;
        public Sprite sprite;
    }
}
