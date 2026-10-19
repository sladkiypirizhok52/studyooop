using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Geography;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using Itmo.ObjectOrientedProgramming.Lab1.Trucks;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class ValueObjectTests
{
    private readonly LabFactory _factory = new LabFactory();

    [Fact]
    public void Constructors_ShouldThrow_WhenValuesAreOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Weight(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Quantity(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Distance(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Speed(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinates(91, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _factory.CreateEmployee(10, 0, 4));
    }
}
