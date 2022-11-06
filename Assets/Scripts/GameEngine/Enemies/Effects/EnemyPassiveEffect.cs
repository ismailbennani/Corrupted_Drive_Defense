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
        [Tooltip("when max stacks is reached, should replace the old stacks with the new ones or keep the old ones")]
        public bool replaceOld = true;

        [Header("Modifiers")]
        public float speedModifier = 1;

        [Header("Poison damage")]
        public int poisonDamage;
        public float poisonPeriod;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static void Apply(EnemyPassiveEffect effect, EnemyPassiveEffectModifier modifier)
        {
            effect.duration *= modifier.durationModifier;
            effect.maxStacks += modifier.maxStacksModifier;
            effect.speedModifier *= modifier.speedModifierModifier;
            effect.poisonDamage += modifier.additionalPoisonDamage;
            effect.poisonPeriod *= modifier.poisonPeriodModifier;
        }

        public string GetTechnicalDescription()
        {
            StringBuilder builder = new();

            if (speedModifier != 1)
            {
                int value = MathUtils.ToSignedPercent(speedModifier);
                builder.AppendLine($"{value:+#;-#;0}% movement speed");
            }
            
            if (poisonDamage > 0)
            {
                builder.AppendLine($"{poisonDamage} damage every {poisonPeriod:0.00}s");
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
