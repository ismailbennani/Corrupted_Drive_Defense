using GameEngine.Map;
using GameEngine.Towers;
using GameEngine.Waves;
using UnityEngine;

namespace GameEngine
{
    [CreateAssetMenu(menuName = "Objects/Game config")]
    public class GameConfig : ScriptableObject
    {
        public MapConfig mapConfig;

        [Header("Processor")]
        public ProcessorConfig processor;
        
        [Header("Towers")]
        public TowerConfig capacitor;

        [Header("Waves")]
        public WaveConfig[] waves;
    }
}
