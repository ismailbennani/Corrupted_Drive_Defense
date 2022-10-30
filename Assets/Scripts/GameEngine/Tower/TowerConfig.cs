using UnityEngine;

namespace GameEngine.Tower
{
    [CreateAssetMenu(menuName = "Objects/Tower config")]
    public class TowerConfig: ScriptableObject
    {
        public Transform prefab;
    }
}
