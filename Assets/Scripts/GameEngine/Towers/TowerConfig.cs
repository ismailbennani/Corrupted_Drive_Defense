using GameComponents;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig: ScriptableObject
    {
        public string towerName;
        public int cost;
        
        [Space(10)]
        public TowerController prefab;
        public Sprite sprite;
    }
}
