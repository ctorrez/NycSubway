using NycSubway.Core.Models;
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
        Task<PagedResult<StationEntranceModel>> GetStationEntrancesAsync(string search, int page, int pageSize);
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

        public async Task<PagedResult<StationEntranceModel>> GetStationEntrancesAsync(string search, int page, int pageSize)
        {
            return await _repo.GetStationEntrancesAsync(search, page, pageSize);
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

            var boundingBox = GeoBoundingBox.FromCenterAndRadius(userLocation, radiusMiles);
            var candidates = await _repo.GetStationEntrancesInBoundingBoxAsync(boundingBox);

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
