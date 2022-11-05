using System;
using GameEngine.Shapes;
using UnityEngine;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerDescription: ICloneable
    {
        [Header("Charge")]
        [Tooltip("Charge consumed from CPU per second")]
        public float frequency;

        public int maxCharge;

        [Header("Target")]
        public TargetType targetType;

        [Tooltip("Ignored when targetType is AreaAtSelf ")]
        public TargetShape range;

        [Tooltip("Ignored when targetType is Single ")]
        public TargetShape targetShape;

        [Header("Base effect")]
        public TowerEffect effect;

        public object Clone()
        {
            TowerDescription result = (TowerDescription)MemberwiseClone();

            result.range = (TargetShape)range.Clone();
            result.targetShape = (TargetShape)targetShape.Clone();
            result.effect = (TowerEffect)effect.Clone();
            
            return result;
        }

        public static TowerDescription Apply(TowerDescription towerDescription, TowerUpgrade upgrade)
        {
            towerDescription.frequency *= upgrade.frequencyMultiplier;
            towerDescription.maxCharge += upgrade.additionalMaxCharge;

            if (upgrade.overrideTargetType != TargetType.None)
            {
                towerDescription.targetType = upgrade.overrideTargetType;
            }

            TargetShape.Apply(towerDescription.range, upgrade.rangeModifier);
            TargetShape.Apply(towerDescription.targetShape, upgrade.targetShapeModifier);
            TowerEffect.Apply(towerDescription.effect, upgrade.effectModifier);
            
            return towerDescription;
        }
    }
}
