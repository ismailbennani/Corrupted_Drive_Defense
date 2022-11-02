using System;
using GameEngine.Enemies.Effects;

namespace GameEngine.Towers
{
    [Serializable]
    public class TowerHitEffect
    {
        public bool enabled;
        public EnemyEffect enemyEffect;
    }
}
