using System.Collections.Generic;

namespace NycSubway.Core.Entities
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
