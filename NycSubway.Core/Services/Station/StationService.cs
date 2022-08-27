using NycSubway.Core.Services.GeoDistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace NycSubway.Core.Services.Station
{
    public class StationService
    {
        public StationService()
        {

        }

        public List<StationEntrance> GetStationEntrances()
        {
            throw new NotImplementedException();
        }

        public GeoDistance GetStationDistance(StationEntrance entrance1, StationEntrance entrance2)
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
