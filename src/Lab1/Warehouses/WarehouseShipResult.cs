using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Warehouses;

public abstract record WarehouseShipResult
{
    private WarehouseShipResult() { }

    public sealed record Success(TimeSpan Duration) : WarehouseShipResult;

    public sealed record NotEnoughItems(SkuSet Missing) : WarehouseShipResult;

    public sealed record Unmanageable(SkuSet Items) : WarehouseShipResult;
}