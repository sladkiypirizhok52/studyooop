using Itmo.ObjectOrientedProgramming.Lab1.Skus;

namespace Itmo.ObjectOrientedProgramming.Lab1.Employees;

public class Staff
{
    public Staff(IReadOnlyCollection<Employee> employees)
    {
        ArgumentNullException.ThrowIfNull(employees);

        // collection literal
        Employees = [..employees];
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

        // LINQ
        //
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

        TimeSpan currentTime = TimeSpan.Zero;

        // [0, 1, ..., shifts.Length - 1]
        // FIFO - first in - first out
        // Employees hanging out next to the truck
        Queue<int> freeEmployees = new Queue<int>([..Enumerable.Range(0, shifts.Length)]);

        // Employees who went to the warehouse to put/take the goods and their return times.
        PriorityQueue<int, TimeSpan> returnTimes = new PriorityQueue<int, TimeSpan>();

        while (!remaining.IsEmpty)
        {
            TimeSpan returningTime = currentTime;
            while (returnTimes.TryPeek(out var returningEmployeeIndex, out returningTime) && returningTime <= currentTime)
            {
                _ = returnTimes.Dequeue();
                freeEmployees.Enqueue(returningEmployeeIndex);
            }

            if (freeEmployees.Any())
            {
                int employeeIndex = freeEmployees.Dequeue();
                EmployeeShift shift = shifts[employeeIndex];
                Employee employee = shift.Employee;
                remaining = shift.MakeTrip(remaining);
                currentTime += employee.HandlingTime;
                returnTimes.Enqueue(employeeIndex, currentTime + shift.Employee.CarryingTime);
            }
            else
            {
                currentTime = returningTime;
            }
            // All goods are distributed across employees "in-advance"
            // Instead, model step-by-step process where the next employee to load/unload is the first
            // one who is next to the car.
            // foreach (EmployeeShift shift in shifts)
            // {
            //     remaining = shift.MakeTrip(remaining);
            // }
        }

        return shifts;
    }

    private bool CanCarry(Sku sku)
    {
        return Employees.Any(employee => sku.Measurements.FitsInto(employee.CarryingLimit));
    }
}
