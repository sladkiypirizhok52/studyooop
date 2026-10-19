using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Skus;

public class Sku
{
    public Sku(SkuId id, string name, Measurements measurements)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (!measurements.IsPositive)
        {
            throw new ArgumentOutOfRangeException(
                nameof(measurements),
                "SKU weight and volume must be positive");
        }

        Id = id;
        Name = name;
        Measurements = measurements;
    }

    public SkuId Id { get; }

    public string Name { get; }

    public Measurements Measurements { get; }
}
