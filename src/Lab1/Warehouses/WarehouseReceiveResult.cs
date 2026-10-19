using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Warehouses;

public abstract record WarehouseReceiveResult
{
    private WarehouseReceiveResult() { }

    public sealed record Success(TimeSpan Duration) : WarehouseReceiveResult;

    public sealed record CapacityExceeded(Measurements Required) : WarehouseReceiveResult;

    public sealed record Unmanageable(SkuSet Items) : WarehouseReceiveResult;
}