using System;
using GameEngine.Enemies.Effects;
using GameEngine.Towers;
using UnityEngine;

namespace GameEngine.Enemies
{
    [Serializable]
    public class EnemyEffectInstance
    {
        public EnemyEffect effect;
        public TowerState source;
        public float creationTime;

        public bool Over => !(effect.duration <= 0) && Time.time >= creationTime + effect.duration;
        
        public EnemyEffectInstance(EnemyEffect effect, TowerState source)
        {
            this.effect = effect;
            this.source = source;
            creationTime = Time.time;
        }
    }
}
