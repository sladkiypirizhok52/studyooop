namespace Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

public readonly record struct Volume
{
    public Volume(decimal cubicMeters)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(cubicMeters);
        CubicMeters = cubicMeters;
    }

    public decimal CubicMeters { get; }

    public static Volume operator +(Volume left, Volume right)
    {
        return new Volume(left.CubicMeters + right.CubicMeters);
    }

    public static Volume operator -(Volume left, Volume right)
    {
        return new Volume(left.CubicMeters - right.CubicMeters);
    }

    public static bool operator <=(Volume left, Volume right)
    {
        return left.CubicMeters <= right.CubicMeters;
    }

    public static bool operator >=(Volume left, Volume right)
    {
        return left.CubicMeters >= right.CubicMeters;
    }

    public Volume TotalFor(int itemCount)
    {
        return new Volume(CubicMeters * itemCount);
    }
}