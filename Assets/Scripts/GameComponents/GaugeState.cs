namespace GameComponents
{
    public struct GaugeState
    {
        public float Min;
        public float Value;
        public float? Max;

        public GaugeState(float value, float? max = null) : this(value, null, max)
        {
        }

        public GaugeState(float value, float? min, float? max)
        {
            Min = min ?? 0;
            Max = max;
            Value = value;
        }
    }
}
