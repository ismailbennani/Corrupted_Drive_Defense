using System;
using System.Linq;
using System.Text;
using GameEngine.Enemies.Effects;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerEffectModifier
    {
        public int additionalDamage;
        public EnemyPassiveEffectModifier[] passiveEffectModifiers = Array.Empty<EnemyPassiveEffectModifier>();
        public EnemyPassiveEffect[] additionalPassiveEffects = Array.Empty<EnemyPassiveEffect>();
        public int additionalRicochet;

        public static TowerEffectModifier CombineInPlace(TowerEffectModifier @this, TowerEffectModifier other)
        {
            @this.additionalDamage += other.additionalDamage;
            @this.passiveEffectModifiers = @this.passiveEffectModifiers.Concat(other.passiveEffectModifiers).ToArray();
            @this.additionalPassiveEffects = @this.additionalPassiveEffects.Concat(other.additionalPassiveEffects).ToArray();
            @this.additionalRicochet += other.additionalRicochet;

            return @this;
        }

        public string GetTechnicalDescription()
        {
            StringBuilder builder = new();

            if (additionalDamage != 0)
            {
                builder.AppendLine($"Damage: {additionalDamage:+#;-#;0}");
            }
            
            if (additionalRicochet != 0)
            {
                builder.AppendLine($"Ricochet: {additionalRicochet:+#;-#;0}");
            }

            foreach (EnemyPassiveEffectModifier passiveModifier in passiveEffectModifiers)
            {
                string passiveEffectModifierTechnicalDescription = passiveModifier.GetTechnicalDescription();
                if (string.IsNullOrEmpty(passiveEffectModifierTechnicalDescription))
                {
                    continue;
                }
                
                builder.AppendLine();
                builder.AppendLine($"Passive: {passiveModifier.effectName}");
                builder.AppendLine(passiveEffectModifierTechnicalDescription);
            }
            
            foreach (EnemyPassiveEffect passiveEffect in additionalPassiveEffects)
            {
                string passiveEffectTechnicalDescription = passiveEffect.GetTechnicalDescription();
                if (string.IsNullOrEmpty(passiveEffectTechnicalDescription))
                {
                    continue;
                }
                
                builder.AppendLine();
                builder.AppendLine($"Passive (new): {passiveEffect.name}");
                builder.AppendLine(passiveEffectTechnicalDescription);
            }
            
            return builder.ToString();
        }
    }
}
