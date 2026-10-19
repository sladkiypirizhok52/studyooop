using Itmo.ObjectOrientedProgramming.Lab1.Employees;
using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Routes;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using Itmo.ObjectOrientedProgramming.Lab1.Warehouses;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class RouteSheetTests
{
    private readonly LabFactory _factory = new LabFactory();
    private readonly Coordinates _origin = new Coordinates(59.9, 30.3);
    private readonly Coordinates _remote = new Coordinates(59.9, 30.4);

    [Fact]
    public void ReceivingStage_ShouldMoveItemsToWarehouse_AndExcludeLastCarryFromDuration()
    {
        Warehouse warehouse = CreateWarehouse(_origin, 0);
        Truck truck = CreateLoadedTruck(_origin, 25);
        var stage = new ReceivingStage(warehouse, _factory.CreateBoxes(25));

        RouteStageResult result = stage.Execute(truck);

        RouteStageResult.Success success = Assert.IsType<RouteStageResult.Success>(result);
        Assert.Equal(TimeSpan.FromMinutes(11), success.Duration);
        Assert.Equal(25, warehouse.Stock.CountOf(_factory.CreateBox()));
        Assert.Empty(truck.Cargo.Batches);
    }

    [Fact]
    public void ReceivingStage_ShouldFail_WhenTruckLacksItems_WarehouseIsFull_OrTruckIsFar()
    {
        var stage = new ReceivingStage(CreateWarehouse(_origin, 0), _factory.CreateBoxes(25));
        var fullWarehouseStage = new ReceivingStage(
            _factory.CreateWarehouse(_origin, CreateStaff(), _factory.CreateEmptySet(), 10),
            _factory.CreateBoxes(25));

        RouteStageResult emptyTruck = stage.Execute(_factory.CreateTruck(_origin));
        RouteStageResult fullWarehouse = fullWarehouseStage.Execute(CreateLoadedTruck(_origin, 25));
        RouteStageResult farTruck = stage.Execute(CreateLoadedTruck(_remote, 25));

        Assert.IsType<RouteStageResult.Failure>(emptyTruck);
        Assert.IsType<RouteStageResult.Failure>(fullWarehouse);
        Assert.IsType<RouteStageResult.Failure>(farTruck);
        Assert.Empty(stage.Warehouse.Stock.Batches);
    }

    [Fact]
    public void ShippingStage_ShouldMoveItemsToTruck_WhenConditionsAreMet()
    {
        Warehouse warehouse = CreateWarehouse(_origin, 25);
        Truck truck = _factory.CreateTruck(_origin);
        var stage = new ShippingStage(warehouse, _factory.CreateBoxes(25));

        RouteStageResult result = stage.Execute(truck);

        RouteStageResult.Success success = Assert.IsType<RouteStageResult.Success>(result);
        Assert.Equal(TimeSpan.FromMinutes(15), success.Duration);
        Assert.Equal(25, truck.Cargo.CountOf(_factory.CreateBox()));
        Assert.Empty(warehouse.Stock.Batches);
    }

    [Fact]
    public void ShippingStage_ShouldFail_WhenWarehouseLacksItems_TruckIsFull_OrTruckIsFar()
    {
        var smallStockStage = new ShippingStage(CreateWarehouse(_origin, 10), _factory.CreateBoxes(25));
        var bigManifestStage = new ShippingStage(CreateWarehouse(_origin, 200), _factory.CreateBoxes(150));
        var stage = new ShippingStage(CreateWarehouse(_origin, 25), _factory.CreateBoxes(25));

        RouteStageResult notEnoughStock = smallStockStage.Execute(_factory.CreateTruck(_origin));
        RouteStageResult truckTooSmall = bigManifestStage.Execute(_factory.CreateTruck(_origin));
        RouteStageResult farTruck = stage.Execute(_factory.CreateTruck(_remote));

        Assert.IsType<RouteStageResult.Failure>(notEnoughStock);
        Assert.IsType<RouteStageResult.Failure>(truckTooSmall);
        Assert.IsType<RouteStageResult.Failure>(farTruck);
        Assert.Equal(25, stage.Warehouse.Stock.CountOf(_factory.CreateBox()));
    }

    [Fact]
    public void Execute_ShouldSumStageDurations_WhenAllStagesSucceed()
    {
        Warehouse source = CreateWarehouse(_origin, 25);
        Warehouse destination = CreateWarehouse(_remote, 0);
        Truck truck = _factory.CreateTruck(_origin);
        TimeSpan travelTime = truck.Speed.TimeToCover(_origin.DistanceTo(_remote));
        var sheet = new RouteSheet([
            new ShippingStage(source, _factory.CreateBoxes(25)),
            new MovementStage(_remote),
            new ReceivingStage(destination, _factory.CreateBoxes(25))]);

        RouteSheetResult result = sheet.Execute(truck);

        RouteSheetResult.Success success = Assert.IsType<RouteSheetResult.Success>(result);
        Assert.Equal(TimeSpan.FromMinutes(15) + travelTime + TimeSpan.FromMinutes(11), success.TotalDuration);
        Assert.Equal(_remote, truck.Location);
        Assert.Empty(source.Stock.Batches);
        Assert.Equal(25, destination.Stock.CountOf(_factory.CreateBox()));
        Assert.Empty(truck.Cargo.Batches);
    }

    [Fact]
    public void Execute_ShouldStopAtFailedStage_AndReportItsNumber()
    {
        Warehouse source = CreateWarehouse(_origin, 25);
        Truck truck = _factory.CreateTruck(_origin);
        var sheet = new RouteSheet([
            new ShippingStage(source, _factory.CreateBoxes(30)),
            new MovementStage(_remote)]);

        RouteSheetResult result = sheet.Execute(truck);

        RouteSheetResult.Failure failure = Assert.IsType<RouteSheetResult.Failure>(result);
        Assert.Equal(1, failure.StageNumber);
        Assert.Equal(_origin, truck.Location);
        Assert.Equal(25, source.Stock.CountOf(_factory.CreateBox()));
    }

    private Staff CreateStaff()
    {
        return new Staff([_factory.CreateEmployee(10, 1, 4)]);
    }

    private Warehouse CreateWarehouse(Coordinates location, int boxesInStock)
    {
        return _factory.CreateWarehouse(
            location,
            CreateStaff(),
            boxesInStock == 0 ? _factory.CreateEmptySet() : _factory.CreateBoxes(boxesInStock));
    }

    private Truck CreateLoadedTruck(Coordinates location, int boxes)
    {
        Truck truck = _factory.CreateTruck(location);
        truck.Load(_factory.CreateBoxes(boxes));

        return truck;
    }
}
