using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Trucks;

public class Truck
{
    public Truck(Measurements capacity, Speed speed, Coordinates location)
    {
        Capacity = capacity;
        Speed = speed;
        Location = location;
        Cargo = new SkuSet([]);
    }

    public Measurements Capacity { get; }

    public Speed Speed { get; }

    public Coordinates Location { get; private set; }

    public SkuSet Cargo { get; private set; }

    public bool CanLoad(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        return Cargo.CombinedWith(items).TotalMeasurements.FitsInto(Capacity);
    }

    public bool Contains(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        return Cargo.Contains(items);
    }

    public TruckLoadResult Load(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        SkuSet loaded = Cargo.CombinedWith(items);

        if (!CanLoad(items))
        {
            return new TruckLoadResult.CapacityExceeded(loaded.TotalMeasurements);
        }

        Cargo = loaded;

        return new TruckLoadResult.Success();
    }

    public TruckUnloadResult Unload(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (!Contains(items))
        {
            return new TruckUnloadResult.NotEnoughItems(items.Without(Cargo));
        }

        Cargo = Cargo.Without(items);

        return new TruckUnloadResult.Success();
    }

    public TimeSpan MoveTo(Coordinates destination)
    {
        Distance distance = Location.DistanceTo(destination);
        Location = destination;

        return Speed.TimeToCover(distance);
    }
}
