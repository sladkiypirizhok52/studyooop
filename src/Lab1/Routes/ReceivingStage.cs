using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using Itmo.ObjectOrientedProgramming.Lab1.Warehouses;
using System.Diagnostics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public class ReceivingStage : IRouteStage
{
    public ReceivingStage(Warehouse warehouse, SkuSet manifest)
    {
        ArgumentNullException.ThrowIfNull(warehouse);
        ArgumentNullException.ThrowIfNull(manifest);

        Warehouse = warehouse;
        Manifest = manifest;
    }

    public Warehouse Warehouse { get; }

    public SkuSet Manifest { get; }

    public RouteStageResult Execute(Truck truck)
    {
        ArgumentNullException.ThrowIfNull(truck);

        if (!Warehouse.IsReachableFrom(truck.Location))
        {
            return new RouteStageResult.Failure("Truck is too far from the warehouse for receiving");
        }

        if (!truck.Contains(Manifest))
        {
            return new RouteStageResult.Failure("Truck does not contain all items from the receiving manifest");
        }

        WarehouseReceiveResult receiveResult = Warehouse.Receive(Manifest);

        if (receiveResult is WarehouseReceiveResult.CapacityExceeded)
        {
            return new RouteStageResult.Failure("Warehouse cannot fit the received items");
        }

        if (receiveResult is WarehouseReceiveResult.Unmanageable)
        {
            return new RouteStageResult.Failure("Warehouse staff cannot carry some of the received items");
        }

        if (receiveResult is not WarehouseReceiveResult.Success received)
        {
            throw new UnreachableException();
        }

        if (truck.Unload(Manifest) is not TruckUnloadResult.Success)
        {
            throw new UnreachableException();
        }

        return new RouteStageResult.Success(received.Duration);
    }
}
