using Itmo.ObjectOrientedProgramming.Lab1.Trucks;

namespace Itmo.ObjectOrientedProgramming.Lab1.Routes;

public interface IRouteStage
{
    RouteStageResult Execute(Truck truck);
}
