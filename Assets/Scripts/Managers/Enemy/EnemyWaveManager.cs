using System;
using System.Collections;
using GameEngine.Enemies;
using GameEngine.Waves;
using UnityEngine;
using UnityEngine.Assertions;

namespace Managers.Enemy
{
    public class EnemyWaveManager: MonoBehaviour
    {
        public bool Spawning { get; private set; }
        public bool Ready => !Spawning && EnemySpawn.GetRemaining() == 0;
        public bool AutoWave { get; private set; }


        [NonSerialized]
        public Vector2Int? Spawn;
        public EnemySpawnApi EnemySpawn;
        public EnemyWaveApi EnemyWaveApi;

        private Coroutine _autoWaveCoroutine;

        void Start()
        {
            Assert.IsNotNull(EnemySpawn);
            Assert.IsNotNull(EnemyWaveApi);
        }

        public void SpawnWave(WaveConfig wave)
        {
            StartCoroutine(SpawnWaveCoroutine(wave));
        }

        public void SetAutoWave(bool auto)
        {
            AutoWave = auto;
            
            if (AutoWave)
            {
                _autoWaveCoroutine = StartCoroutine(AutoTriggerWave());
            }
            else
            {
                if (_autoWaveCoroutine != null)
                {
                    StopCoroutine(_autoWaveCoroutine);
                }
            }
        }
        
        private IEnumerator SpawnWaveCoroutine(WaveConfig wave)
        {
            Assert.IsFalse(Spawn == null);
            
            Spawning = true;

            float delay = wave.frequency == 0 ? 1 : 1 / wave.frequency;

            foreach (EnemyConfig enemy in wave.enemies)
            {
                EnemySpawn.SpawnEnemy(enemy, Spawn.Value);
                yield return new WaitForSeconds(delay);
            }

            Spawning = false;

            if (AutoWave)
            {
                StartCoroutine(AutoTriggerWave());
            }
        }

        private IEnumerator AutoTriggerWave()
        {
            while (!Ready)
            {
                yield return null;
            }

            if (AutoWave)
            {
                EnemyWaveApi.SpawnNextWave();
            }
        }
    }
}
