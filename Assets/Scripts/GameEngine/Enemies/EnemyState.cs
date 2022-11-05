﻿using System;
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
        public int strength;

        [Space(10)]
        public EnemyCharacteristics characteristics;

        public List<EnemyEffectInstance> effects = new();

        [Space(10)]
        public EnemyConfig config;


        public EnemyState(long id, EnemyConfig config)
        {
            this.id = id;
            this.config = config;

            strength = ComputeStrength(config);

            UpdateCharacteristics();
        }

        public void SetConfig(EnemyConfig newConfig)
        {
            config = newConfig;
            UpdateCharacteristics();
        }

        public void AddEffect(EnemyPassiveEffect passiveEffect, TowerState source)
        {
            if (passiveEffect.maxStacks > 0)
            {
                EnemyEffectInstance[] stacks = effects.Where(e => e.passiveEffect.name == passiveEffect.name).OrderBy(e => e.creationTime).ToArray();

                if (stacks.Length >= passiveEffect.maxStacks)
                {
                    int toRemove = stacks.Length - passiveEffect.maxStacks + 1;
                    foreach (EnemyEffectInstance stack in stacks.Take(toRemove))
                    {
                        effects.Remove(stack);
                    }
                }
            }

            effects.Add(new EnemyEffectInstance(passiveEffect, source));
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
            characteristics = new EnemyCharacteristics(config);
            characteristics.Apply(effects.Select(e => e.passiveEffect).ToArray());
        }

        private static int ComputeStrength(EnemyConfig config)
        {
            int result = 0;
            EnemyConfig c = config;
            while (c.child != null)
            {
                result++;
                c = c.child;
            }

            return result;
        }
    }
}
