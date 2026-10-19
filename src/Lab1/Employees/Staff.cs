using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Employees;

public class Staff
{
    public Staff(IReadOnlyCollection<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);

        Employees = employees.ToArray();
    }

    public IReadOnlyCollection<Employee> Employees { get; }

    public StaffWorkResult CalculateLoadingTime(SkuSet items)
    {
        return CalculateWorkTime(items, shift => shift.CalculateLoadingTime());
    }

    public StaffWorkResult CalculateUnloadingTime(SkuSet items)
    {
        return CalculateWorkTime(items, shift => shift.CalculateUnloadingTime());
    }

    private StaffWorkResult CalculateWorkTime(SkuSet items, Func<EmployeeShift, TimeSpan> shiftTime)
    {
        ArgumentNullException.ThrowIfNull(items);

        var unmanageable = new SkuSet(items.Batches.Where(batch => !CanCarry(batch.Sku)).ToArray());

        if (!unmanageable.IsEmpty)
        {
            return new StaffWorkResult.Unmanageable(unmanageable);
        }

        TimeSpan duration = DistributeWork(items)
            .Select(shiftTime)
            .DefaultIfEmpty(TimeSpan.Zero)
            .Max();

        return new StaffWorkResult.Success(duration);
    }

    private EmployeeShift[] DistributeWork(SkuSet items)
    {
        EmployeeShift[] shifts = Employees.Select(employee => new EmployeeShift(employee)).ToArray();
        SkuSet remaining = items;

        while (!remaining.IsEmpty)
        {
            foreach (EmployeeShift shift in shifts)
            {
                remaining = shift.MakeTrip(remaining);
            }
        }

        return shifts;
    }

    private bool CanCarry(Sku sku)
    {
        return Employees.Any(employee => sku.Measurements.FitsInto(employee.CarryingLimit));
    }
}
