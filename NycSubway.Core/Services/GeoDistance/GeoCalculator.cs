using NycSubway.Core.Services.Station;
using System;

namespace NycSubway.Core.Services.GeoDistance
{
    public interface IGeoCalculator
    {
        double CalculateDistance(GeoPoint origin, GeoPoint destination, UnitOfLength unit);
    }

    public class HaversineGeoCalculator : IGeoCalculator
    {
        private const double EarthRadiusMiles = 3958.8;

        public double CalculateDistance(GeoPoint origin, GeoPoint destination, UnitOfLength unit)
        {
            if (origin == null || destination == null)
            {
                return 0.0;
            }

            if (unit == null)
            {
                unit = UnitOfLength.Miles;
            }

            // Fast exit for identical points
            if (Math.Abs(origin.Latitude - destination.Latitude) < 1e-9 &&
                Math.Abs(origin.Longitude - destination.Longitude) < 1e-9)
            {
                return 0.0;
            }

            var lat1Rad = Math.PI * origin.Latitude / 180.0;
            var lat2Rad = Math.PI * destination.Latitude / 180.0;
            var dLat = Math.PI * (destination.Latitude - origin.Latitude) / 180.0;
            var dLon = Math.PI * (destination.Longitude - origin.Longitude) / 180.0;

            var a = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0);

            a = Math.Clamp(a, 0.0, 1.0);

            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            var distanceInMiles = EarthRadiusMiles * c;

            return unit.ConvertFromMiles(distanceInMiles);
        }
    }
}
