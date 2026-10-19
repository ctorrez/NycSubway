using NycSubway.Core.Services.GeoDistance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NycSubway.Core.Services.Station
{
    public interface IStationService
    {
        Task<List<StationEntranceModel>> GetStationEntrancesAsync();
        GeoDistance GetStationDistance(StationEntranceModel entrance1, StationEntranceModel entrance2);
    }

    public class StationService : IStationService
    {
        private readonly IStationRepo _repo;

        public StationService(IStationRepo repo)
        {
            _repo = repo;
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

            var c1 = new Coordinates(entrance1.Point.Latitude, entrance1.Point.Longitude);
            var c2 = new Coordinates(entrance2.Point.Latitude, entrance2.Point.Longitude);

            var distance = c1.DistanceTo(c2, UnitOfLength.Miles);

            return new GeoDistance
            {
                Unit = "Miles",
                Value = distance
            };
        }
    }
}
