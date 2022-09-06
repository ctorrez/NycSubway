using NycSubway.Core.Services.GeoDistance;
using System.Collections.Generic;

namespace NycSubway.Core.Services.Station
{
    public class StationService
    {
        private readonly IStationRepo _repo;

        public StationService(IStationRepo repo)
        {
            _repo = repo;
        }

        public List<StationEntranceModel> GetStationEntrances()
        {
            return _repo.GetStationEntrances();
        }

        public GeoDistance GetStationDistance(StationEntranceModel entrance1, StationEntranceModel entrance2)
        {
            var c1 = new Coordinates(entrance1.Point.Latitude, entrance1.Point.Longitude);
            var c2 = new Coordinates(entrance2.Point.Latitude, entrance2.Point.Longitude);

            var distance = c1.DistanceTo(c2, UnitOfLength.Miles);

            return new GeoDistance()
            {
                Unit = "Miles",
                Value = distance
            };
        }
    }
}
