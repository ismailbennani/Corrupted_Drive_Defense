using System;

namespace GameEngine.Processor
{
    public enum ProcessorUpgradeType
    {
        ChargeRate,
        MaxCharge
    }

    public static class ProcessorUpgradeTypeExtensions
    {
        public static string GetDisplayName(this ProcessorUpgradeType upgrade)
        {
            return upgrade switch
            {
                ProcessorUpgradeType.ChargeRate => "charge rate",
                ProcessorUpgradeType.MaxCharge => "max charge",
                _ => throw new ArgumentOutOfRangeException(nameof(upgrade), upgrade, null)
            };
        }
    }
}
