using System;

namespace GameEngine.Towers
{
    public enum TargetStrategy
    {
        First,
        Last,
        Close,
        Strong
    }

    public static class TargetStrategyExtensions
    {
        public static string GetDescription(this TargetStrategy targetStrategy)
        {
            return targetStrategy switch
            {
                TargetStrategy.First => "First",
                TargetStrategy.Last => "Last",
                TargetStrategy.Close => "Close",
                TargetStrategy.Strong => "Strong",
                _ => throw new ArgumentOutOfRangeException(nameof(targetStrategy), targetStrategy, null)
            };
        }
    }
}
