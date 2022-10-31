using UnityEngine;

namespace GameEngine.Tower
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig: ScriptableObject
    {
        public string name;
        public int cost;
        
        [Space(10)]
        public Transform prefab;
        public Sprite sprite;
    }
}
