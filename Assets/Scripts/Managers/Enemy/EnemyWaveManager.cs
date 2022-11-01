using System;
using System.Collections;
using GameEngine;
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

        [NonSerialized]
        public Vector2Int? Spawn;
        public EnemySpawnApi EnemySpawn;

        void Start()
        {
            Assert.IsFalse(Spawn == null);
            Assert.IsNotNull(EnemySpawn);
        }

        public void SpawnWave(WaveConfig wave)
        {
            StartCoroutine(SpawnWaveCoroutine(wave));
        }
        
        private IEnumerator SpawnWaveCoroutine(WaveConfig wave)
        {
            Spawning = true;

            float delay = wave.frequency == 0 ? 1 : 1 / wave.frequency;

            foreach (EnemyConfig enemy in wave.enemies)
            {
                EnemySpawn.SpawnEnemy(enemy, Spawn.Value);
                yield return new WaitForSeconds(delay);
            }

            Spawning = false;
        }
    }
}
