using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies.Effects;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerEffect : ICloneable
    {
        public int damage;

        [Tooltip("Effect applied to all enemies when they are hit")]
        public EnemyPassiveEffect[] passiveEffects = Array.Empty<EnemyPassiveEffect>();

        [Header("Special effects")]
        [Tooltip("Number of additional enemies targeted by a hit, only used when target type is Single")]
        public int ricochet;

        public object Clone()
        {
            TowerEffect result = (TowerEffect)MemberwiseClone();

            result.passiveEffects = result.passiveEffects.Select(e => (EnemyPassiveEffect)e.Clone()).ToArray();

            return result;
        }

        public static void Apply(TowerEffect effect, TowerEffectModifier modifier)
        {
            effect.damage += modifier.additionalDamage;

            effect.passiveEffects = effect.passiveEffects.Concat(modifier.additionalPassiveEffects).ToArray();

            foreach (EnemyPassiveEffect passiveEffect in effect.passiveEffects)
            {
                IEnumerable<EnemyPassiveEffectModifier> modifiers = modifier.passiveEffectModifiers.Where(m => m.effectName == passiveEffect.name);
                foreach (EnemyPassiveEffectModifier effectModifier in modifiers)
                {
                    EnemyPassiveEffect.Apply(passiveEffect, effectModifier);
                }
            }

            effect.ricochet += modifier.additionalRicochet;
        }
    }
}
