using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Employees;

public class EmployeeShift
{
    public EmployeeShift(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        Employee = employee;
    }

    public Employee Employee { get; }

    public int TripCount { get; private set; }

    public SkuSet MakeTrip(SkuSet pile)
    {
        ArgumentNullException.ThrowIfNull(pile);

        SkuSet taken = Employee.PickUp(pile);

        if (taken.IsEmpty)
        {
            return pile;
        }

        TripCount++;

        return pile.Without(taken);
    }

    public TimeSpan CalculateLoadingTime()
    {
        return Employee.TripTime * TripCount;
    }

    public TimeSpan CalculateUnloadingTime()
    {
        if (TripCount == 0)
        {
            return TimeSpan.Zero;
        }

        return CalculateLoadingTime() - Employee.CarryingTime;
    }
}
