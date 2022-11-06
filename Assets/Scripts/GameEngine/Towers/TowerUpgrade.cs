﻿using System;
using GameEngine.Enemies.Effects;
using GameEngine.Shapes;
using UnityEngine;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower upgrade")]
    public class TowerUpgrade : ScriptableObject
    {
        public string upgradeName;
        public int cost;
        public Sprite sprite;

        [Header("Charge")]
        public float fullChargeDelayMultiplier = 1;
        public int additionalMaxCharge;

        [Header("Target")]
        public TargetType overrideTargetType;

        public TargetShapeModifier rangeModifier;
        public TargetShapeModifier targetShapeModifier;

        [Header("Effects")]
        [Tooltip("Modifiy existing effect")]
        public TowerEffectModifier effectModifier;

        public static TowerUpgrade CombineInPlace(TowerUpgrade @this, TowerUpgrade other)
        {
            @this.cost += other.cost;
            @this.fullChargeDelayMultiplier *= other.fullChargeDelayMultiplier;
            @this.additionalMaxCharge += other.additionalMaxCharge;

            if (@this.overrideTargetType != TargetType.None
                && other.overrideTargetType != TargetType.None
                && @this.overrideTargetType != other.overrideTargetType)
            {
                Debug.LogWarning(
                    $"Conflict between upgrades {@this.upgradeName} and {other.upgradeName}: target shape {@this.overrideTargetType} != {other.overrideTargetType}"
                );
            }
            else if (other.overrideTargetType != TargetType.None)
            {
                @this.overrideTargetType = other.overrideTargetType;
            }

            TargetShapeModifier.CombineInPlace(@this.rangeModifier, other.rangeModifier);
            TargetShapeModifier.CombineInPlace(@this.targetShapeModifier, other.targetShapeModifier);

            TowerEffectModifier.CombineInPlace(@this.effectModifier, other.effectModifier);

            return @this;
        }

        public static TowerUpgrade GetEmpty()
        {
            TowerUpgrade result = CreateInstance<TowerUpgrade>();

            result.upgradeName = "Aggregated upgrade";
            result.cost = 0;
            result.sprite = null;
            result.fullChargeDelayMultiplier = 1;
            result.additionalMaxCharge = 0;
            result.overrideTargetType = TargetType.None;
            result.rangeModifier = new TargetShapeModifier();
            result.targetShapeModifier = new TargetShapeModifier();
            result.effectModifier = new TowerEffectModifier { passiveEffectModifiers = Array.Empty<EnemyPassiveEffectModifier>() };

            return result;
        }
    }
}
