using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Trucks;

public abstract record TruckUnloadResult
{
    private TruckUnloadResult() { }

    public sealed record Success : TruckUnloadResult;

    public sealed record NotEnoughItems(SkuSet Missing) : TruckUnloadResult;
}