namespace Itmo.ObjectOrientedProgramming.Lab1.Geography;

public readonly record struct Distance
{
    public Distance(double meters)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(meters);
        Meters = meters;
    }

    public double Meters { get; }

    public static bool operator <=(Distance left, Distance right)
    {
        return left.Meters <= right.Meters;
    }

    public static bool operator >=(Distance left, Distance right)
    {
        return left.Meters >= right.Meters;
    }
}