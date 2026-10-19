namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public abstract record RouteSheetResult
{
    private RouteSheetResult() { }

    public sealed record Success(TimeSpan TotalDuration) : RouteSheetResult;

    public sealed record Failure(int StageNumber, string Message) : RouteSheetResult;
}