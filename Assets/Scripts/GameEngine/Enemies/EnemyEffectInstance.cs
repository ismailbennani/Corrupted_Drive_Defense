using System;
using GameEngine.Enemies.Effects;
using GameEngine.Towers;
using UnityEngine;

namespace GameEngine.Enemies
{
    [Serializable]
    public class EnemyEffectInstance
    {
        public EnemyPassiveEffect passiveEffect;
        public long sourceId;
        public float creationTime;

        public float lastPoisonTime;
        
        public bool Over => !(passiveEffect.duration <= 0) && Time.time >= creationTime + passiveEffect.duration;

        public EnemyEffectInstance(EnemyPassiveEffect passiveEffect, TowerState source)
        {
            this.passiveEffect = passiveEffect;
            sourceId = source.id;
            creationTime = Time.time;
            lastPoisonTime = Time.time;
        }
    }
}
