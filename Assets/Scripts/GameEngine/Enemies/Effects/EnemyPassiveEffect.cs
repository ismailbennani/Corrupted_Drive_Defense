using System;
using UnityEngine;

namespace GameEngine.Enemies.Effects
{
    [Serializable]
    public class EnemyPassiveEffect : ICloneable
    {
        [Tooltip("Unique name for this effect")]
        public string name;
        [Tooltip("in seconds")]
        public float duration;
        public int maxStacks;

        [Header("Modifiers")]
        public float speedModifier = 1;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static void Apply(EnemyPassiveEffect effect, EnemyPassiveEffectModifier modifier)
        {
            effect.duration *= modifier.durationModifier;
            effect.maxStacks += modifier.maxStacksModifier;
            effect.speedModifier *= modifier.speedModifier;
        }
    }
}
