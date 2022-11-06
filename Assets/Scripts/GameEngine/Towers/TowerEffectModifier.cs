using System;
using System.Linq;
using GameEngine.Enemies.Effects;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerEffectModifier
    {
        public int additionalDamage;
        public EnemyPassiveEffectModifier[] passiveEffectModifiers = Array.Empty<EnemyPassiveEffectModifier>();
        public EnemyPassiveEffect[] additionalEffects = Array.Empty<EnemyPassiveEffect>();
        public int additionalRicochet;

        public static TowerEffectModifier CombineInPlace(TowerEffectModifier @this, TowerEffectModifier other)
        {
            @this.additionalDamage += other.additionalDamage;
            @this.passiveEffectModifiers = @this.passiveEffectModifiers.Concat(other.passiveEffectModifiers).ToArray();
            @this.additionalEffects = @this.additionalEffects.Concat(other.additionalEffects).ToArray();
            @this.additionalRicochet += other.additionalRicochet;

            return @this;
        }
    }
}
