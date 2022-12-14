using System;
using System.Text;
using UnityEngine;
using Utils;

namespace GameEngine.Enemies.Effects
{
    [Serializable]
    public class EnemyPassiveEffectModifier
    {
        public string effectName;
        public float durationModifier = 1;
        public int maxStacksModifier;
        
        [Header("Modifiers modifiers")]
        public float speedModifierModifier = 1;

        [Header("Poison damage modifiers")]
        public int additionalPoisonDamage;
        public float poisonPeriodModifier = 1;

        public static EnemyPassiveEffectModifier CombineInPlace(EnemyPassiveEffectModifier @this, EnemyPassiveEffectModifier other)
        {
            @this.durationModifier *= other.durationModifier;
            @this.maxStacksModifier += other.maxStacksModifier;
            @this.speedModifierModifier *= other.speedModifierModifier;

            return @this;
        }

        public string GetTechnicalDescription()
        {
            StringBuilder builder = new();
            
            if (speedModifierModifier != 1)
            {
                int value = MathUtils.ToSignedPercent(speedModifierModifier);
                builder.AppendLine($"{-value:+#;-#;0}% movement speed decrease");
            }
            
            if (additionalPoisonDamage != 0)
            {
                builder.AppendLine($"{-additionalPoisonDamage:+#;-#;0} damage");
            }
            
            if (poisonPeriodModifier != 1)
            {
                int value = MathUtils.ToSignedPercent(poisonPeriodModifier);
                builder.AppendLine($"{value:+#;-#;0}% damage period");
            }

            if (durationModifier != 1)
            {
                if (durationModifier == 0)
                {
                    builder.AppendLine("Duration: ∞"); 
                }
                else
                {
                    int value = MathUtils.ToSignedPercent(durationModifier);
                    builder.AppendFormat($"{value:+#;-#}% duration");
                }
            }

            if (maxStacksModifier > 0)
            {
                builder.AppendLine($"{maxStacksModifier:+#;-#;0} max stacks");
            }

            return builder.ToString();
        }
    }
}
