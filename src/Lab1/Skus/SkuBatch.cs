using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Skus;

public class SkuBatch
{
    public SkuBatch(Sku sku, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(sku);

        Sku = sku;
        Quantity = quantity;
        TotalMeasurements = sku.Measurements.TotalFor(quantity.Value);
    }

    public Sku Sku { get; }

    public Quantity Quantity { get; }

    public Measurements TotalMeasurements { get; }
}
