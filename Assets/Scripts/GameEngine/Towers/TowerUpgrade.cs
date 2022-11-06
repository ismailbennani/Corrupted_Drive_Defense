using System;
using System.Text;
using GameEngine.Enemies.Effects;
using GameEngine.Shapes;
using UnityEngine;
using Utils;

namespace GameEngine.Towers
{
    [CreateAssetMenu(menuName = "Objects/Tower upgrade")]
    public class TowerUpgrade : ScriptableObject
    {
        public string upgradeName;
        public int cost;
        [TextArea]
        public string description;
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

        public string GetTechnicalDescription()
        {
            StringBuilder builder = new();

            if (fullChargeDelayMultiplier != 1)
            {
                float value = MathUtils.ToSignedPercent(1 / fullChargeDelayMultiplier);
                builder.AppendLine($"{value:+#;-#;0}% charge rate");
            }
            
            if (additionalMaxCharge != 0)
            {
                builder.AppendLine($"{additionalMaxCharge:+#;-#;0} max charge");
            }
            
            if (overrideTargetType != TargetType.None)
            {
                builder.AppendLine();
                builder.AppendLine("Target type");
                builder.AppendLine($"{overrideTargetType}");
            }

            string rangeTechnicalDescription = rangeModifier.GetTechnicalDescription();
            if (!string.IsNullOrEmpty(rangeTechnicalDescription))
            {
                builder.AppendLine();
                builder.AppendLine("Range");
                builder.AppendLine(rangeTechnicalDescription);
            }

            string targetShapeTechnicalDescription = targetShapeModifier.GetTechnicalDescription();
            if (!string.IsNullOrEmpty(targetShapeTechnicalDescription))
            {
                builder.AppendLine();
                builder.AppendLine("Target shape");
                builder.AppendLine(targetShapeTechnicalDescription);
            }

            string effectModifierTechnicalDescription = effectModifier.GetTechnicalDescription();
            if (!string.IsNullOrEmpty(effectModifierTechnicalDescription))
            {
                builder.AppendLine();
                builder.AppendLine("Effects");
                builder.AppendLine(effectModifierTechnicalDescription);
            }

            return builder.ToString();
        }
    }
}
