using GameComponents;
using GameEngine.Shapes;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig: ScriptableObject
    {
        public string towerName;
        public Shape targetArea;
        
        [Header("Ticks")]
        public float frequency;
        public int maxTicks;

        [Space(10)]
        public TowerController prefab;
        public Sprite sprite;
    }
}
