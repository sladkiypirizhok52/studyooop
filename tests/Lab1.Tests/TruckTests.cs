using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class TruckTests
{
    private readonly LabFactory _factory = new LabFactory();
    private readonly Coordinates _origin = new Coordinates(59.9, 30.3);

    [Fact]
    public void Load_ShouldUpdateCargo_AndFailWhenCapacityExceeded()
    {
        Truck truck = _factory.CreateTruck(_origin);

        TruckLoadResult firstLoad = truck.Load(_factory.CreateBoxes(60));
        TruckLoadResult secondLoad = truck.Load(_factory.CreateBoxes(50));

        Assert.IsType<TruckLoadResult.Success>(firstLoad);
        Assert.IsType<TruckLoadResult.CapacityExceeded>(secondLoad);
        Assert.Equal(60, truck.Cargo.CountOf(_factory.CreateBox()));
    }

    [Fact]
    public void Unload_ShouldReduceCargo_AndFailWhenItemsAreMissing()
    {
        Truck truck = _factory.CreateTruck(_origin);
        truck.Load(_factory.CreateBoxes(20));

        TruckUnloadResult firstUnload = truck.Unload(_factory.CreateBoxes(5));
        TruckUnloadResult secondUnload = truck.Unload(_factory.CreateBoxes(30));

        Assert.IsType<TruckUnloadResult.Success>(firstUnload);
        TruckUnloadResult.NotEnoughItems failure = Assert.IsType<TruckUnloadResult.NotEnoughItems>(secondUnload);
        Assert.Equal(15, failure.Missing.CountOf(_factory.CreateBox()));
        Assert.Equal(15, truck.Cargo.CountOf(_factory.CreateBox()));
    }

    [Fact]
    public void MoveTo_ShouldUpdateLocation_AndComputeTravelTimeFromDistanceAndSpeed()
    {
        var equator = new Coordinates(0, 0);
        var oneDegreeNorth = new Coordinates(1, 0);
        Truck truck = _factory.CreateTruck(equator);

        TimeSpan duration = truck.MoveTo(oneDegreeNorth);

        Assert.Equal(oneDegreeNorth, truck.Location);
        Assert.Equal(111195.0, equator.DistanceTo(oneDegreeNorth).Meters, 0);
        Assert.Equal(11119.5, duration.TotalSeconds, 1);
    }
}
