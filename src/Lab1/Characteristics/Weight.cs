namespace Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

public readonly record struct Weight
{
    public Weight(decimal kilograms)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(kilograms);

        Kilograms = kilograms;
    }

    public decimal Kilograms { get; }

    public static Weight operator +(Weight left, Weight right)
    {
        return new Weight(left.Kilograms + right.Kilograms);
    }

    public static Weight operator -(Weight left, Weight right)
    {
        return new Weight(left.Kilograms - right.Kilograms);
    }

    public static bool operator <=(Weight left, Weight right)
    {
        return left.Kilograms <= right.Kilograms;
    }

    public static bool operator >=(Weight left, Weight right)
    {
        return left.Kilograms >= right.Kilograms;
    }

    public Weight TotalFor(int itemCount)
    {
        return new Weight(Kilograms * itemCount);
    }
}