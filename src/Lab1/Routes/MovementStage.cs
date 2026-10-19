using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;

namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public class MovementStage : IRouteStage
{
    public MovementStage(Coordinates destination)
    {
        Destination = destination;
    }

    public Coordinates Destination { get; }

    public RouteStageResult Execute(Truck truck)
    {
        ArgumentNullException.ThrowIfNull(truck);

        TimeSpan duration = truck.MoveTo(Destination);

        return new RouteStageResult.Success(duration);
    }
}
