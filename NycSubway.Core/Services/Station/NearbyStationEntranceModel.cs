namespace NycSubway.Core.Services.Station
{
    public class NearbyStationEntranceModel
    {
        public string Name { get; set; }
        public GeoPoint Point { get; set; }
        public double DistanceInMiles { get; set; }
    }
}
