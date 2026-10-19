using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Trucks;

public abstract record TruckLoadResult
{
    private TruckLoadResult() { }

    public sealed record Success : TruckLoadResult;

    public sealed record CapacityExceeded(Measurements Required) : TruckLoadResult;
}
