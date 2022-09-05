using System;
using System.Collections.Generic;
using System.Text;

namespace NycSubway.Data.Entities
{
    public class StationEntrance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual List<AppUserStationEntrance> AppUserStationEntrances { get; set; }
    }
}
