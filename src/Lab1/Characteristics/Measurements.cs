namespace Itmo.ObjectOrientedProgramming.Lab1.Characteristics;

// VolumeWeightCharacteristics
public readonly record struct Measurements
{
    public Measurements(Weight weight, Volume volume)
    {
        Weight = weight;
        Volume = volume;
        IsPositive = (weight.Kilograms > 0) && (volume.CubicMeters > 0);
    }

    public Weight Weight { get; }

    public Volume Volume { get; }

    public bool IsPositive { get; }

    public static Measurements operator +(Measurements left, Measurements right)
    {
        return new Measurements(left.Weight + right.Weight, left.Volume + right.Volume);
    }

    public static Measurements operator -(Measurements left, Measurements right)
    {
        return new Measurements(left.Weight - right.Weight, left.Volume - right.Volume);
    }

    public bool FitsInto(Measurements limit)
    {
        return (Weight <= limit.Weight) && (Volume <= limit.Volume);
    }

    public Measurements TotalFor(int itemCount)
    {
        return new Measurements(Weight.TotalFor(itemCount), Volume.TotalFor(itemCount));
    }

    public int CountFitting(Measurements itemMeasurements, int availableCount)
    {
        if (!itemMeasurements.IsPositive)
        {
            throw new ArgumentOutOfRangeException(nameof(itemMeasurements), "Item measurements must be positive");
        }

        decimal byWeight = Math.Floor(Weight.Kilograms / itemMeasurements.Weight.Kilograms);
        decimal byVolume = Math.Floor(Volume.CubicMeters / itemMeasurements.Volume.CubicMeters);
        decimal fitting = Math.Min(Math.Min(byWeight, byVolume), availableCount);

        return decimal.ToInt32(fitting);
    }
}
