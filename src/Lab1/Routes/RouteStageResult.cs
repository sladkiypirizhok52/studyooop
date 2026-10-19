namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public abstract record RouteStageResult
{
    private RouteStageResult() { }

    public sealed record Success(TimeSpan Duration) : RouteStageResult;

    public sealed record Failure(string Message) : RouteStageResult;
}
