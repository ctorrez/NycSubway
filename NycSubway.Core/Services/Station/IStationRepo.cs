using System.Collections.Generic;

namespace NycSubway.Core.Services.Station
{
    public interface IStationRepo
    {
        List<StationEntrance> GetStationEntrances();
    }
}
