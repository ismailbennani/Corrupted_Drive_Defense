using System;
using UnityEngine;

namespace GameEngine.Processor
{
    [Serializable]
    public class ProcessorUpgradeConfig
    {
        public string upgradeName;
        public int cost;
        public int upgrade;
        public int max;
        public Sprite sprite;
    }
}
