using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Employees;

public class Employee
{
    public Employee(Measurements carryingLimit, TimeSpan handlingTime, TimeSpan carryingTime)
    {
        if (!carryingLimit.IsPositive)
        {
            throw new ArgumentOutOfRangeException(nameof(carryingLimit), "Carrying limit must be positive");
        }

        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(handlingTime, TimeSpan.Zero);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(carryingTime, TimeSpan.Zero);

        CarryingLimit = carryingLimit;
        HandlingTime = handlingTime;
        CarryingTime = carryingTime;
        TripTime = handlingTime + carryingTime;
    }

    public Measurements CarryingLimit { get; }

    public TimeSpan HandlingTime { get; }

    public TimeSpan CarryingTime { get; }

    public TimeSpan TripTime { get; }

    public SkuSet PickUp(SkuSet pile)
    {
        ArgumentNullException.ThrowIfNull(pile);

        var cart = new SkuSet([]);

        foreach (SkuBatch batch in pile.Batches)
        {
            cart = cart.CombinedWith(FittingPartOf(batch, cart));
        }

        return cart;
    }

    private SkuSet FittingPartOf(SkuBatch batch, SkuSet cart)
    {
        Measurements freeSpace = CarryingLimit - cart.TotalMeasurements;
        int count = freeSpace.CountFitting(batch.Sku.Measurements, batch.Quantity.Value);

        if (count == 0)
        {
            return new SkuSet([]);
        }

        return new SkuSet([new SkuBatch(batch.Sku, new Quantity(count))]);
    }
}
