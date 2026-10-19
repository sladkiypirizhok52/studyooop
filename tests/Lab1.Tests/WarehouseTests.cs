using Itmo.ObjectOrientedProgramming.Lab1.Employees;
using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Warehouses;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class WarehouseTests
{
    private readonly LabFactory _factory = new LabFactory();
    private readonly Coordinates _origin = new Coordinates(59.9, 30.3);

    [Fact]
    public void Receive_ShouldIncreaseStock_AndFailWhenCapacityExceeded()
    {
        Warehouse warehouse = _factory.CreateWarehouse(_origin, CreateStaff(), _factory.CreateBoxes(5), 20);

        WarehouseReceiveResult firstReceive = warehouse.Receive(_factory.CreateBoxes(10));
        WarehouseReceiveResult secondReceive = warehouse.Receive(_factory.CreateBoxes(10));

        WarehouseReceiveResult.Success success = Assert.IsType<WarehouseReceiveResult.Success>(firstReceive);
        Assert.Equal(TimeSpan.FromMinutes(1), success.Duration);
        Assert.IsType<WarehouseReceiveResult.CapacityExceeded>(secondReceive);
        Assert.Equal(15, warehouse.Stock.CountOf(_factory.CreateBox()));
    }

    [Fact]
    public void Ship_ShouldDecreaseStock_AndFailWhenItemsAreMissing()
    {
        Warehouse warehouse = _factory.CreateWarehouse(_origin, CreateStaff(), _factory.CreateBoxes(25));

        WarehouseShipResult firstShip = warehouse.Ship(_factory.CreateBoxes(10));
        WarehouseShipResult secondShip = warehouse.Ship(_factory.CreateBoxes(20));

        WarehouseShipResult.Success success = Assert.IsType<WarehouseShipResult.Success>(firstShip);
        Assert.Equal(TimeSpan.FromMinutes(5), success.Duration);
        WarehouseShipResult.NotEnoughItems failure = Assert.IsType<WarehouseShipResult.NotEnoughItems>(secondShip);
        Assert.Equal(5, failure.Missing.CountOf(_factory.CreateBox()));
        Assert.Equal(15, warehouse.Stock.CountOf(_factory.CreateBox()));
    }

    [Fact]
    public void Receive_ShouldFailAndKeepStock_WhenStaffCannotCarryItem()
    {
        Warehouse warehouse = _factory.CreateWarehouse(_origin, CreateStaff(), _factory.CreateEmptySet());

        WarehouseReceiveResult result = warehouse.Receive(_factory.CreateHeavyBoxes(3));

        Assert.IsType<WarehouseReceiveResult.Unmanageable>(result);
        Assert.Empty(warehouse.Stock.Batches);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenInitialStockExceedsCapacity()
    {
        Staff staff = CreateStaff();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _factory.CreateWarehouse(_origin, staff, _factory.CreateBoxes(20), 10));
    }

    private Staff CreateStaff()
    {
        return new Staff([_factory.CreateEmployee(10, 1, 4)]);
    }
}
