using GameEngine.Enemies;
using UnityEngine;

namespace GameEngine.Waves
{
    [CreateAssetMenu(menuName = "Objects/Wave config")]
    public class WaveConfig: ScriptableObject
    {
        public EnemyConfig[] enemies;
        public float frequency;
    }
}
