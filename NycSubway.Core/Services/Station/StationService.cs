using NycSubway.Core.Services.GeoDistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.Core.Services.Station
{
    public interface IStationService
    {
        Task<List<StationEntranceModel>> GetStationEntrancesAsync();
        GeoDistance GetStationDistance(StationEntranceModel entrance1, StationEntranceModel entrance2);
        Task<List<NearbyStationEntranceModel>> GetNearbyStationEntrancesAsync(
            GeoPoint userLocation, double radiusMiles, int limit = 10);
    }

    public class StationService : IStationService
    {
        private readonly IStationRepo _repo;
        private readonly IGeoCalculator _geoCalculator;

        public StationService(IStationRepo repo, IGeoCalculator geoCalculator)
        {
            _repo = repo;
            _geoCalculator = geoCalculator;
        }

        public async Task<List<StationEntranceModel>> GetStationEntrancesAsync()
        {
            return await _repo.GetStationEntrancesAsync();
        }

        public GeoDistance GetStationDistance(StationEntranceModel entrance1, StationEntranceModel entrance2)
        {
            if (entrance1?.Point == null || entrance2?.Point == null)
            {
                return new GeoDistance
                {
                    Unit = "Miles",
                    Value = 0
                };
            }

            var distance = _geoCalculator.CalculateDistance(entrance1.Point, entrance2.Point, UnitOfLength.Miles);

            return new GeoDistance
            {
                Unit = "Miles",
                Value = distance
            };
        }

        public async Task<List<NearbyStationEntranceModel>> GetNearbyStationEntrancesAsync(
            GeoPoint userLocation, double radiusMiles, int limit = 10)
        {
            if (userLocation == null || radiusMiles <= 0)
            {
                return new List<NearbyStationEntranceModel>();
            }

            // Approximate degree deltas for bounding-box database filtering:
            // ~69 miles per degree of latitude
            // ~69 * cos(lat) miles per degree of longitude
            const double milesPerDegreeLat = 69.0;
            var cosLat = Math.Cos(userLocation.Latitude * Math.PI / 180.0);
            var milesPerDegreeLon = 69.0 * Math.Max(Math.Abs(cosLat), 0.01);

            var latDelta = radiusMiles / milesPerDegreeLat;
            var lonDelta = radiusMiles / milesPerDegreeLon;

            var candidates = await _repo.GetStationEntrancesInBoundingBoxAsync(
                userLocation.Latitude - latDelta,
                userLocation.Latitude + latDelta,
                userLocation.Longitude - lonDelta,
                userLocation.Longitude + lonDelta);

            var nearbyList = candidates
                .Select(s =>
                {
                    var point = new GeoPoint(s.Latitude, s.Longitude);
                    var distance = _geoCalculator.CalculateDistance(userLocation, point, UnitOfLength.Miles);
                    return new NearbyStationEntranceModel
                    {
                        Name = s.Name,
                        Point = point,
                        DistanceInMiles = Math.Round(distance, 3)
                    };
                })
                .Where(x => x.DistanceInMiles <= radiusMiles)
                .OrderBy(x => x.DistanceInMiles)
                .Take(limit)
                .ToList();

            return nearbyList;
        }
    }
}
