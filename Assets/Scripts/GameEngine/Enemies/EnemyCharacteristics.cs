using System;
using GameEngine.Enemies.Effects;

namespace GameEngine.Enemies
{
    [Serializable]
    public class EnemyCharacteristics
    {
        public int hp;
        public float speed;

        public EnemyCharacteristics(EnemyConfig config)
        {
            hp = config.hp;
            speed = config.speed;
        }

        public void Apply(params EnemyPassiveEffect[] effects)
        {
            foreach (EnemyPassiveEffect effect in effects)
            {
                speed *= effect.speedModifier;
            }
        }
    }
}
