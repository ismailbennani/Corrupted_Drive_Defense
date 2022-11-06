using System;
using GameEngine.Enemies.Effects;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerEffect : ICloneable
    {
        public int damage;

        [Header("Passive effect")]
        public bool applyPassiveEffect;

        [Tooltip("Effect applied to all enemies when they are hit")]
        public EnemyPassiveEffect passiveEffect;

        [Header("Special effects")]
        [Tooltip("Number of additional enemies targeted by a hit")]
        public int ricochet;

        public object Clone()
        {
            TowerEffect result = (TowerEffect)MemberwiseClone();

            result.passiveEffect = (EnemyPassiveEffect)passiveEffect.Clone();

            return result;
        }

        public static void Apply(TowerEffect effect, TowerEffectModifier modifier)
        {
            effect.damage += modifier.additionalDamage;

            if (effect.applyPassiveEffect)
            {
                EnemyPassiveEffect.Apply(effect.passiveEffect, modifier.passiveEffectModifier);
            }

            effect.ricochet += modifier.additionalRicochet;
        }
    }
}
