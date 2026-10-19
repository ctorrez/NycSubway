using NycSubway.Core.Services.Station;
using System;

namespace NycSubway.Core.Models
{
    /// <summary>
    /// Represents a 2D geospatial rectangular bounding box.
    /// </summary>
    public readonly record struct GeoBoundingBox(
        double MinLatitude,
        double MaxLatitude,
        double MinLongitude,
        double MaxLongitude)
    {
        public GeoPoint SouthWest => new GeoPoint(MinLatitude, MinLongitude);
        public GeoPoint NorthEast => new GeoPoint(MaxLatitude, MaxLongitude);

        /// <summary>
        /// Creates a bounding box around a center point with a given radius in miles.
        /// </summary>
        public static GeoBoundingBox FromCenterAndRadius(GeoPoint center, double radiusMiles)
        {
            if (center == null)
            {
                throw new ArgumentNullException(nameof(center));
            }

            if (radiusMiles <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radiusMiles), "Radius must be positive.");
            }

            // Approximate degree deltas for bounding-box database filtering:
            // ~69 miles per degree of latitude
            // ~69 * cos(lat) miles per degree of longitude
            const double milesPerDegreeLat = 69.0;
            var cosLat = Math.Cos(center.Latitude * Math.PI / 180.0);
            var milesPerDegreeLon = 69.0 * Math.Max(Math.Abs(cosLat), 0.01);

            var latDelta = radiusMiles / milesPerDegreeLat;
            var lonDelta = radiusMiles / milesPerDegreeLon;

            return new GeoBoundingBox(
                MinLatitude: center.Latitude - latDelta,
                MaxLatitude: center.Latitude + latDelta,
                MinLongitude: center.Longitude - lonDelta,
                MaxLongitude: center.Longitude + lonDelta);
        }

        /// <summary>
        /// Checks whether a given point falls inside this bounding box.
        /// </summary>
        public bool Contains(GeoPoint point) =>
            point != null &&
            point.Latitude >= MinLatitude && point.Latitude <= MaxLatitude &&
            point.Longitude >= MinLongitude && point.Longitude <= MaxLongitude;
    }
}
