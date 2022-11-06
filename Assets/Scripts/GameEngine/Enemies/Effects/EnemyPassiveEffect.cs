using System;
using System.Text;
using UnityEngine;
using Utils;

namespace GameEngine.Enemies.Effects
{
    [Serializable]
    public class EnemyPassiveEffect : ICloneable
    {
        [Tooltip("Unique name for this effect")]
        public string name;
        [Tooltip("in seconds")]
        public float duration = 1;
        public int maxStacks = 1;

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
            effect.speedModifier *= modifier.speedModifierModifier;
        }

        public string GetTechnicalDescription()
        {
            StringBuilder builder = new();

            if (speedModifier != 1)
            {
                int value = MathUtils.ToSignedPercent(speedModifier);
                builder.AppendLine($"{value:+#;-#;0}% movement speed");
            }
            
            builder.AppendLine($"Duration: {(duration == 0 ? '∞' : $"{duration:0.00}s")}");

            if (maxStacks > 0)
            {
                builder.AppendLine($"Max stacks: {maxStacks}");
            }

            return builder.ToString();
        }
    }
}
