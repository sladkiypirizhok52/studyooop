using Itmo.ObjectOrientedProgramming.Lab1.Characteristics;
using Itmo.ObjectOrientedProgramming.Lab1.Skus;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class SkuTests
{
    [Fact]
    public void Constructor_ShouldCreateSku_WhenAttributesAreValid()
    {
        var id = new SkuId(1);
        var measurements = new Measurements(new Weight(2.5m), new Volume(0.01m));

        var sku = new Sku(id, "Milk", measurements);

        Assert.Equal(id, sku.Id);
        Assert.Equal("Milk", sku.Name);
        Assert.Equal(measurements, sku.Measurements);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenAttributesAreInvalid()
    {
        var id = new SkuId(1);
        var valid = new Measurements(new Weight(2.5m), new Volume(0.01m));
        var zeroWeight = new Measurements(new Weight(0), new Volume(0.01m));
        var zeroVolume = new Measurements(new Weight(2.5m), new Volume(0));

        Assert.Throws<ArgumentOutOfRangeException>(() => new Sku(id, "Milk", zeroWeight));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Sku(id, "Milk", zeroVolume));
        Assert.Throws<ArgumentException>(() => new Sku(id, "   ", valid));
    }
}
