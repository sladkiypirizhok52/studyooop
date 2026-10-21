using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Skus;

public record class SkuSet
{
    public SkuSet(IReadOnlyCollection<SkuBatch> batches)
    {
        ArgumentNullException.ThrowIfNull(batches);

        Batches = [.. batches
            .GroupBy(batch => batch.Sku.Id)
            .Select(sameSku => new SkuBatch(
                sameSku.First().Sku,
                new Quantity(sameSku.Sum(batch => batch.Quantity.Value))))];

        IsEmpty = Batches.Count == 0;

        TotalMeasurements = Batches.Aggregate(
            new Measurements(),
            (acc, batch) => acc + batch.TotalMeasurements
        );
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

        SkuBatch[] remaining = [.. Batches
            .Where(batch => batch.Quantity.Value > other.CountOf(batch.Sku))
            .Select(batch => new SkuBatch(
                batch.Sku,
                new Quantity(batch.Quantity.Value - other.CountOf(batch.Sku))))];

        return new SkuSet(remaining);
    }
}
