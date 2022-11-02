using System;
using UnityEngine;

namespace GameEngine.Enemies.Effects
{
    [Serializable]
    public class EnemyEffect
    {
        [Tooltip("Unique name for this effect")]
        public string name;
        [Tooltip("in seconds")]
        public float duration;
        public int maxStacks;
        
        [Header("Modifiers")]
        public float speedModifier = 1;

        public static EnemyCharacteristics Apply(EnemyConfig config, params EnemyEffect[] effects)
        {
            EnemyCharacteristics result = new(config);

            foreach (EnemyEffect effect in effects)
            {
                Apply(result, effect);
            }
            
            return result;
        }

        private static void Apply(EnemyCharacteristics result, EnemyEffect effects)
        {
            result.speed *= effects.speedModifier;
        }
    }
}
