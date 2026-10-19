using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Employees;

public abstract record StaffWorkResult
{
    private StaffWorkResult() { }

    public sealed record Success(TimeSpan Duration) : StaffWorkResult;

    public sealed record Unmanageable(SkuSet Items) : StaffWorkResult;
}
