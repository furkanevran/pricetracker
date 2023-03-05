using System;

namespace PriceTracker.Entities;

[Flags]
public enum AlertStrategies
{
    EMAIL = 1,
    SMS = 2,
    PUSH_NOTIFICATION = 4,
    TELEGRAM = 16,
}

public static class AlertStrategiesExtensions
{
    public static bool HasFlag(this AlertStrategies alertStrategies, AlertStrategies flag)
    {
        return (alertStrategies & flag) == flag;
    }
}