namespace Itmo.ObjectOrientedProgramming.Lab1.Geography;

public readonly record struct Coordinates
{
    private const double MaxLatitude = 90;
    private const double MaxLongitude = 180;
    private const double EarthRadiusMeters = 6_371_000;

    public Coordinates(double latitude, double longitude)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(latitude, -MaxLatitude);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(latitude, MaxLatitude);
        ArgumentOutOfRangeException.ThrowIfLessThan(longitude, -MaxLongitude);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(longitude, MaxLongitude);
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }

    public double Longitude { get; }

    public Distance DistanceTo(Coordinates other)
    {
        double latitude = double.DegreesToRadians(Latitude);
        double otherLatitude = double.DegreesToRadians(other.Latitude);
        double latitudeDelta = otherLatitude - latitude;
        double longitudeDelta = double.DegreesToRadians(other.Longitude - Longitude);

        double latitudeTerm = Math.Pow(Math.Sin(latitudeDelta / 2), 2);
        double longitudeTerm = Math.Pow(Math.Sin(longitudeDelta / 2), 2);
        double haversine = latitudeTerm + (Math.Cos(latitude) * Math.Cos(otherLatitude) * longitudeTerm);

        double centralAngle = 2 * Math.Asin(Math.Sqrt(haversine));

        return new Distance(EarthRadiusMeters * centralAngle);
    }
}