using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using System.Diagnostics;

namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public class RouteSheet
{
    public RouteSheet(IReadOnlyCollection<IRouteStage> stages)
    {
        ArgumentNullException.ThrowIfNull(stages);

        Stages = [..stages];
    }

    public IReadOnlyCollection<IRouteStage> Stages { get; }

    public RouteSheetResult Execute(Truck truck)
    {
        ArgumentNullException.ThrowIfNull(truck);

        TimeSpan total = TimeSpan.Zero;
        int stageNumber = 0;

        foreach (IRouteStage stage in Stages)
        {
            stageNumber++;

            RouteStageResult stageResult = stage.Execute(truck);

            if (stageResult is RouteStageResult.Failure failure)
            {
                return new RouteSheetResult.Failure(stageNumber, failure.Message);
            }

            if (stageResult is not RouteStageResult.Success success)
            {
                throw new UnreachableException();
            }

            total += success.Duration;
        }

        return new RouteSheetResult.Success(total);
    }
}
