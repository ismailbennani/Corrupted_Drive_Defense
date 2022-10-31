using System;

namespace GameComponents
{
    [Serializable]
    public struct GaugeState
    {
        public float min;
        public float value;
        public float max;

        public GaugeState(float value, float? max = null) : this(value, null, max)
        {
        }

        public GaugeState(float value, float? min, float? max)
        {
            this.min = min ?? 0;
            this.max = max ?? -1;
            this.value = value;
        }
    }
}
