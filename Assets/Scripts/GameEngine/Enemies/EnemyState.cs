using System;
using UnityEngine;

namespace GameEngine.Enemies
{
    [Serializable]
    public struct EnemyState
    {
        public long id;
        public int hp;
        public int pathIndex;
        public float pathCellCompletion;

        [Space(10)]
        public EnemyConfig config;
    }
}
