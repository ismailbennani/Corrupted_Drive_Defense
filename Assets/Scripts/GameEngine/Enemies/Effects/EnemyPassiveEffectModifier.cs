using System;

namespace GameEngine.Enemies.Effects
{
    [Serializable]
    public class EnemyPassiveEffectModifier
    {
        public float durationModifier = 1;
        public int maxStacksModifier = 0;
        public float speedModifier = 1;

        public static void CombineInPlace(EnemyPassiveEffectModifier @this, EnemyPassiveEffectModifier other)
        {
            @this.durationModifier *= other.durationModifier;
            @this.maxStacksModifier += other.maxStacksModifier;
            @this.speedModifier *= other.speedModifier;
        }
    }
}
