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
        public float frequencyMultiplier = 1;

        public int additionalMaxCharge = 0;

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
            @this.frequencyMultiplier *= other.frequencyMultiplier;
            @this.additionalMaxCharge += other.additionalMaxCharge;

            if (@this.overrideTargetType != TargetType.None
                && other.overrideTargetType != TargetType.None
                && @this.overrideTargetType != other.overrideTargetType)
            {
                Debug.LogWarning(
                    $"Conflict between upgrades {@this.upgradeName} and {other.upgradeName}: target shape {@this.overrideTargetType} != {other.overrideTargetType}");
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
    }
}
