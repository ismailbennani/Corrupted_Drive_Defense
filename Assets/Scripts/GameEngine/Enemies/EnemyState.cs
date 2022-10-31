using System;
using UnityEngine;

namespace GameEngine.Enemies
{
    [Serializable]
    public class EnemyState
    {
        public long id;
        public int hp;
        public int pathIndex;
        public float pathCellCompletion;

        [Space(10)]
        public EnemyConfig config;

        public EnemyState(long id, EnemyConfig config)
        {
            this.id = id;
            this.config = config;
        }
    }
}
