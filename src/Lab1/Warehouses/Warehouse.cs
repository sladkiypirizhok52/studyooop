using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Employees;
using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using System.Diagnostics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Warehouses;

public class Warehouse
{
    private const double ArrivalDistanceMeters = 10;

    public Warehouse(WarehouseId id, Coordinates location, Measurements capacity, Staff staff, SkuSet stock)
    {
        ArgumentNullException.ThrowIfNull(staff);
        ArgumentNullException.ThrowIfNull(stock);

        if (!stock.TotalMeasurements.FitsInto(capacity))
        {
            throw new ArgumentOutOfRangeException(nameof(stock), "Initial stock does not fit into warehouse capacity");
        }

        Id = id;
        Location = location;
        Capacity = capacity;
        Staff = staff;
        Stock = stock;
    }

    public WarehouseId Id { get; }

    public Coordinates Location { get; }

    public Measurements Capacity { get; }

    public Staff Staff { get; }

    public SkuSet Stock { get; private set; }

    public bool IsReachableFrom(Coordinates location)
    {
        return location.DistanceTo(Location) <= new Distance(ArrivalDistanceMeters);
    }

    public WarehouseReceiveResult Receive(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        SkuSet updated = Stock.CombinedWith(items);

        if (!updated.TotalMeasurements.FitsInto(Capacity))
        {
            return new WarehouseReceiveResult.CapacityExceeded(updated.TotalMeasurements);
        }

        StaffWorkResult workResult = Staff.CalculateUnloadingTime(items);

        if (workResult is StaffWorkResult.Unmanageable unmanageable)
        {
            return new WarehouseReceiveResult.Unmanageable(unmanageable.Items);
        }

        if (workResult is not StaffWorkResult.Success success)
        {
            throw new UnreachableException();
        }

        Stock = updated;

        return new WarehouseReceiveResult.Success(success.Duration);
    }

    public WarehouseShipResult Ship(SkuSet items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (!Stock.Contains(items))
        {
            return new WarehouseShipResult.NotEnoughItems(items.Without(Stock));
        }

        StaffWorkResult workResult = Staff.CalculateLoadingTime(items);

        if (workResult is StaffWorkResult.Unmanageable unmanageable)
        {
            return new WarehouseShipResult.Unmanageable(unmanageable.Items);
        }

        if (workResult is not StaffWorkResult.Success success)
        {
            throw new UnreachableException();
        }

        Stock = Stock.Without(items);

        return new WarehouseShipResult.Success(success.Duration);
    }
}
