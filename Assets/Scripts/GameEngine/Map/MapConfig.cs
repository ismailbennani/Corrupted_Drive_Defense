using UnityEngine;

namespace GameEngine.Map
{
    [CreateAssetMenu(menuName = "Objects/Map")]
    public class MapConfig : ScriptableObject
    {
        [Header("Description")]
        public Transform gameObject;
        
        [Space(10)]
        public Vector2Int topLeftCorner;
        public Vector2Int mapSize;
        
        [Space(10)]
        public Vector2Int[] path;
        public BoundsInt[] walls;

        [Space(10)]
        public Vector2Int processorPosition;
    }
}
