using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Employees;

// public class EmployeeWorkOperation
// {
//     private enum Stage { Started, OneWay, Roundrip }

//     private Stage stage = Stage.Started;

//     public DateTimeOffset Start { get; } = DateTimeOffset.Now;

//     public DateTimeOffset OneWayFinish { get; private set; }

//     public DateTimeOffset RoundtripFinish { get; private set; }

//     public EmployeeWorkOperation(Employee employee, Sku sku)
//     {

//     }

//     public void FinishOneWay()
//     {
//         if (this.stage != Stage.Started)
//         {
//             throw new ArgumentException("Cannot finish one way");
//         }

//         this.OneWayFinish = DateTimeOffset.Now;
//         this.stage = Stage.OneWay;
//     }

//     public void FinishRoundtrip()
//     {
//         if (this.stage != Stage.OneWay)
//         {
//             throw new ArgumentException("Cannot finish roundtrip");
//         }

//         this.RoundtripFinish = DateTimeOffset.Now;
//         this.stage = Stage.Roundrip;
//     }

//     public TimeSpan FactualDuration => this.stage switch
//     {
//         Stage.Started => TimeSpan.Zero,

//         Stage.OneWay => this.OneWayFinish - this.Start,

//         Stage.Roundrip => this.RoundtripFinish - this.Start,
//     };
// }

// EmployeeLoadUnloadOperation
// EmployeeLoadUnloadWork
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
