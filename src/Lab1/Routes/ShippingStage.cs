using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using Itmo.ObjectOrientedProgramming.Lab1.Warehouses;
using System.Diagnostics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public class ShippingStage : IRouteStage
{
    public ShippingStage(Warehouse warehouse, SkuSet manifest)
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
            return new RouteStageResult.Failure("Truck is too far from the warehouse for shipping");
        }

        if (!truck.CanLoad(Manifest))
        {
            return new RouteStageResult.Failure("Truck cannot fit the shipped items");
        }

        WarehouseShipResult shipResult = Warehouse.Ship(Manifest);

        if (shipResult is WarehouseShipResult.NotEnoughItems)
        {
            return new RouteStageResult.Failure("Warehouse does not contain all items from the shipping manifest");
        }

        if (shipResult is WarehouseShipResult.Unmanageable)
        {
            return new RouteStageResult.Failure("Warehouse staff cannot carry some of the shipped items");
        }

        if (shipResult is not WarehouseShipResult.Success shipped)
        {
            throw new UnreachableException();
        }

        if (truck.Load(Manifest) is not TruckLoadResult.Success)
        {
            throw new UnreachableException();
        }

        return new RouteStageResult.Success(shipped.Duration);
    }
}
