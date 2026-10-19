using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Employees;
using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using Itmo.ObjectOrientedProgramming.Lab1.Warehouses;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class LabFactory
{
    private const int TruckCapacityInBoxes = 100;
    private const int WarehouseCapacityInBoxes = 1000;
    private const double TruckSpeedMetersPerSecond = 10;

    public Sku CreateBox()
    {
        return new Sku(new SkuId(1), "Box", CreateBoxMeasurements(1));
    }

    public SkuSet CreateBoxes(int count)
    {
        return new SkuSet([new SkuBatch(CreateBox(), new Quantity(count))]);
    }

    public SkuSet CreateEmptySet()
    {
        return new SkuSet([]);
    }

    public Sku CreateHeavyBox()
    {
        return new Sku(new SkuId(2), "Heavy box", new Measurements(new Weight(50), new Volume(0.5m)));
    }

    public SkuSet CreateHeavyBoxes(int count)
    {
        return new SkuSet([new SkuBatch(CreateHeavyBox(), new Quantity(count))]);
    }

    public Employee CreateEmployee(int boxesPerTrip, int handlingMinutes, int carryingMinutes)
    {
        return new Employee(
            CreateBoxMeasurements(boxesPerTrip),
            TimeSpan.FromMinutes(handlingMinutes),
            TimeSpan.FromMinutes(carryingMinutes));
    }

    public Truck CreateTruck(Coordinates location)
    {
        return new Truck(CreateBoxMeasurements(TruckCapacityInBoxes), new Speed(TruckSpeedMetersPerSecond), location);
    }

    public Warehouse CreateWarehouse(Coordinates location, Staff staff, SkuSet stock)
    {
        return CreateWarehouse(location, staff, stock, WarehouseCapacityInBoxes);
    }

    public Warehouse CreateWarehouse(Coordinates location, Staff staff, SkuSet stock, int capacityInBoxes)
    {
        return new Warehouse(new WarehouseId(1), location, CreateBoxMeasurements(capacityInBoxes), staff, stock);
    }

    private Measurements CreateBoxMeasurements(int boxes)
    {
        return new Measurements(new Weight(boxes), new Volume(0.1m * boxes));
    }
}
