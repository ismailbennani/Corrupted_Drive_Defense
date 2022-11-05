using System;
using GameEngine.Enemies.Effects;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerEffectModifier
    {
        public int additionalDamage;
        public EnemyPassiveEffectModifier passiveEffectModifier;
        public int additionalRicochet;

        public static TowerEffectModifier CombineInPlace(TowerEffectModifier @this, TowerEffectModifier other)
        {
            @this.additionalDamage += other.additionalDamage;

            EnemyPassiveEffectModifier.CombineInPlace(@this.passiveEffectModifier, other.passiveEffectModifier);

            @this.additionalRicochet += other.additionalRicochet;

            return @this;
        }
    }
}
