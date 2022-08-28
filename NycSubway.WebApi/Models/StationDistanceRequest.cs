using NycSubway.Core.Services.Station;

namespace NycSubway.WebApi.Models
{
    public class StationDistanceRequest
    {
        public StationEntrance Entrance1 { get; set; }
        public StationEntrance Entrance2 { get; set; }
    }
}
