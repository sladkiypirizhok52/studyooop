using Itmo.ObjectOrientedProgramming.Lab1.Employees;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class StaffTests
{
    private readonly LabFactory _factory = new LabFactory();

    [Fact]
    public void MakeTrip_ShouldTakeOnlyWhatFitsIntoCart_AndCountTrips()
    {
        var shift = new EmployeeShift(_factory.CreateEmployee(10, 1, 4));

        SkuSet afterFirstTrip = shift.MakeTrip(_factory.CreateBoxes(25));
        SkuSet afterSecondTrip = shift.MakeTrip(afterFirstTrip);
        SkuSet afterThirdTrip = shift.MakeTrip(afterSecondTrip);

        Assert.Equal(15, afterFirstTrip.CountOf(_factory.CreateBox()));
        Assert.Equal(5, afterSecondTrip.CountOf(_factory.CreateBox()));
        Assert.Empty(afterThirdTrip.Batches);
        Assert.Equal(3, shift.TripCount);
    }

    [Fact]
    public void CalculateTime_ShouldExcludeLastCarryOnlyFromUnloading_ForSingleEmployee()
    {
        var staff = new Staff([_factory.CreateEmployee(10, 1, 4)]);

        StaffWorkResult loading = staff.CalculateLoadingTime(_factory.CreateBoxes(25));
        StaffWorkResult unloading = staff.CalculateUnloadingTime(_factory.CreateBoxes(25));

        StaffWorkResult.Success loadingSuccess = Assert.IsType<StaffWorkResult.Success>(loading);
        StaffWorkResult.Success unloadingSuccess = Assert.IsType<StaffWorkResult.Success>(unloading);
        Assert.Equal(TimeSpan.FromMinutes(15), loadingSuccess.Duration);
        Assert.Equal(TimeSpan.FromMinutes(11), unloadingSuccess.Duration);
    }

    [Fact]
    public void CalculateTime_ShouldBeLimitedByBusiestEmployee_WhenSeveralWorkInParallel()
    {
        var staff = new Staff([_factory.CreateEmployee(10, 1, 4), _factory.CreateEmployee(5, 1, 2)]);

        StaffWorkResult loading = staff.CalculateLoadingTime(_factory.CreateBoxes(25));
        StaffWorkResult unloading = staff.CalculateUnloadingTime(_factory.CreateBoxes(25));

        StaffWorkResult.Success loadingSuccess = Assert.IsType<StaffWorkResult.Success>(loading);
        StaffWorkResult.Success unloadingSuccess = Assert.IsType<StaffWorkResult.Success>(unloading);
        Assert.Equal(TimeSpan.FromMinutes(10), loadingSuccess.Duration);
        Assert.Equal(TimeSpan.FromMinutes(6), unloadingSuccess.Duration);
    }

    [Fact]
    public void CalculateTime_ShouldFail_WhenItemDoesNotFitAnyEmployee()
    {
        var staff = new Staff([_factory.CreateEmployee(10, 1, 4)]);

        StaffWorkResult result = staff.CalculateLoadingTime(_factory.CreateHeavyBoxes(3));

        StaffWorkResult.Unmanageable failure = Assert.IsType<StaffWorkResult.Unmanageable>(result);
        Assert.Equal(3, failure.Items.CountOf(_factory.CreateHeavyBox()));
    }
}
