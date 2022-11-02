using System;

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
    }
}
