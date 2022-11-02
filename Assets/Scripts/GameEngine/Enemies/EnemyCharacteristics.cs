using System;

namespace GameEngine.Enemies
{
    [Serializable]
    public class EnemyCharacteristics
    {
        public float speed;

        public EnemyCharacteristics(EnemyConfig config)
        {
            speed = config.speed;
        }
    }
}
