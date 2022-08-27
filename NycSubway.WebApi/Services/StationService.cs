using NycSubway.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.WebApi.Services
{
    public class StationService
    {
        public List<StationEntrance> GetStationEntrances()
        {
            return new List<StationEntrance>()
            {
                new StationEntrance()
                {
                    Name = "Birchall Ave & Sagamore St at NW corner",
                    Point = new GeoPoint()
                    {
                        Latitude = -73.86835600032798,
                        Longitude = 40.84916900104506
                    }
                }
            };
        }
    }
}
