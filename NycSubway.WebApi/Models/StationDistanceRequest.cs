using NycSubway.Core.Services.Station;

namespace NycSubway.WebApi.Models
{
    public class StationDistanceRequest
    {
        public StationEntranceModel Entrance1 { get; set; }
        public StationEntranceModel Entrance2 { get; set; }
    }
}
