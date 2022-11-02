using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Enemies.Effects;
using GameEngine.Towers;
using UnityEngine;

namespace GameEngine.Enemies
{
    [Serializable]
    public class EnemyState
    {
        public long id;
        public int pathIndex;
        public float pathCellCompletion;

        [Space(10)]
        public EnemyCharacteristics characteristics;

        public List<EnemyEffectInstance> effects = new();

        [Space(10)]
        public EnemyConfig config;


        public EnemyState(long id, EnemyConfig config)
        {
            this.id = id;
            this.config = config;

            UpdateCharacteristics();
        }

        public void SetConfig(EnemyConfig newConfig)
        {
            config = newConfig;
            UpdateCharacteristics();
        }

        public void AddEffect(EnemyEffect effect, TowerState source)
        {
            if (effect.maxStacks > 0)
            {
                EnemyEffectInstance[] stacks = effects.Where(e => e.effect.name == effect.name).OrderBy(e => e.creationTime).ToArray();

                if (stacks.Length >= effect.maxStacks)
                {
                    int toRemove = stacks.Length - effect.maxStacks + 1;
                    foreach (EnemyEffectInstance stack in stacks.Take(toRemove))
                    {
                        effects.Remove(stack);
                    }
                }
            }

            effects.Add(new EnemyEffectInstance(effect, source));
            UpdateCharacteristics();
        }

        public void Update()
        {
            if (effects.Any(e => e.Over))
            {
                effects.RemoveAll(e => e.Over);
                UpdateCharacteristics();
            }
        }

        private void UpdateCharacteristics()
        {
            characteristics = EnemyEffect.Apply(config, effects.Select(e => e.effect).ToArray());
        }
    }
}
