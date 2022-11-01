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
        public bool Ongoing { get; private set; }
        public bool Ready => !Ongoing;

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
            Ongoing = true;

            foreach (EnemyConfig enemy in wave.enemies)
            {
                EnemySpawn.SpawnEnemy(enemy, Spawn.Value);
                yield return new WaitForSeconds(1f);
            }

            Ongoing = false;
        }
    }
}
