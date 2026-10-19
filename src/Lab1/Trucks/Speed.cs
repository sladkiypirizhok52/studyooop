using Itmo.ObjectOrientedProgramming.Lab1.Geography;

namespace Itmo.ObjectOrientedProgramming.Lab1.Trucks;

public readonly record struct Speed
{
    public Speed(double metersPerSecond)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(metersPerSecond);
        MetersPerSecond = metersPerSecond;
    }

    public double MetersPerSecond { get; }

    public TimeSpan TimeToCover(Distance distance)
    {
        return TimeSpan.FromSeconds(distance.Meters / MetersPerSecond);
    }
}