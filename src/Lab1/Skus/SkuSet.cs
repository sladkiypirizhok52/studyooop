using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Skus;

public class SkuSet
{
    public SkuSet(IReadOnlyCollection<SkuBatch> batches)
    {
        ArgumentNullException.ThrowIfNull(batches);

        Batches = batches
            .GroupBy(batch => batch.Sku.Id)
            .Select(sameSku => new SkuBatch(
                sameSku.First().Sku,
                new Quantity(sameSku.Sum(batch => batch.Quantity.Value))))
            .ToArray();

        IsEmpty = Batches.Count == 0;

        TotalMeasurements = new Measurements(
            new Weight(Batches.Sum(batch => batch.TotalMeasurements.Weight.Kilograms)),
            new Volume(Batches.Sum(batch => batch.TotalMeasurements.Volume.CubicMeters)));
    }

    public IReadOnlyCollection<SkuBatch> Batches { get; }

    public bool IsEmpty { get; }

    public Measurements TotalMeasurements { get; }

    public SkuSet CombinedWith(SkuSet other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return new SkuSet(Batches.Concat(other.Batches).ToArray());
    }

    public int CountOf(Sku sku)
    {
        ArgumentNullException.ThrowIfNull(sku);

        return Batches
            .Where(batch => batch.Sku.Id == sku.Id)
            .Sum(batch => batch.Quantity.Value);
    }

    public bool Contains(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        return items.Without(this).IsEmpty;
    }

    public SkuSet Without(SkuSet other)
    {
        ArgumentNullException.ThrowIfNull(other);

        SkuBatch[] remaining = Batches
            .Where(batch => batch.Quantity.Value > other.CountOf(batch.Sku))
            .Select(batch => new SkuBatch(
                batch.Sku,
                new Quantity(batch.Quantity.Value - other.CountOf(batch.Sku))))
            .ToArray();

        return new SkuSet(remaining);
    }
}
