using GameEngine.Map;
using GameEngine.Processor;
using GameEngine.Towers;
using GameEngine.Waves;
using UnityEngine;

namespace GameEngine
{
    [CreateAssetMenu(menuName = "Objects/Game config")]
    public class GameConfig : ScriptableObject
    {
        public MapConfig mapConfig;
        public int startingMoney;
        public float towerResellCoefficient = 0.8f;

        [Header("Processor")]
        public ProcessorConfig processor;

        [Header("Towers")]
        public TowerConfig[] towers;

        [Header("Waves")]
        public WaveConfig[] waves;

        [Space(10)]
        [Header("UI: shape preview")]
        public SpriteRenderer cellPrefab;
        public Color shapePreviewOkColor;
        public Color shapePreviewErrorColor;
    }
}
