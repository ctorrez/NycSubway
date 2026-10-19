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
    }
}
