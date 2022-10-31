using System;
using UnityEngine;

namespace GameComponents
{
    [Serializable]
    public struct GaugeState
    {
        public float min;
        public float value;
        public float max;
        
        public bool Full => value >= max;

        public GaugeState(float value, float? max = null) : this(value, null, max)
        {
        }

        public GaugeState(float value, float? min, float? max)
        {
            this.min = min ?? 0;
            this.max = max ?? -1;
            this.value = value;
        }

        public float Add(float charge)
        {
            float newValue = max > 0 ? Mathf.Min(value + charge, max) : value + charge;
            float added = newValue - value;
            Set(newValue);
            return added;
        }

        public float Consume(float charge)
        {
            float actualConsumption = Mathf.Min(charge, value);
            Set(value - actualConsumption);
            return actualConsumption;
        }

        public void Set(float newValue)
        {
            value = max > 0 ? Mathf.Clamp(newValue, min, max) : Mathf.Max(newValue, min);
        }

        public void Clear()
        {
            Set(min);
        }
    }
}
