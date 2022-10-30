using GameEngine.Map;
using GameEngine.Tower;
using UnityEngine;

namespace GameEngine
{
    [CreateAssetMenu(menuName = "Objects/Game config")]
    public class GameConfig : ScriptableObject
    {
        public MapConfig mapConfig;

        [Header("Towers")]
        public TowerConfig processor;
        public TowerConfig capacitor;
    }
}
