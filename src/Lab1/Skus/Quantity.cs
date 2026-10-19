namespace Itmo.ObjectOrientedProgramming.Lab1.Skus;

public readonly record struct Quantity
{
    public Quantity(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        Value = value;
    }

    public int Value { get; }
}