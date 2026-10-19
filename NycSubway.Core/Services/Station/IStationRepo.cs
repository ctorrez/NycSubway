using NycSubway.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NycSubway.Core.Services.Station
{
    public interface IStationRepo
    {
        Task<List<StationEntranceModel>> GetStationEntrancesAsync();
        Task<List<StationEntrance>> GetStationEntrancesInBoundingBoxAsync(
            double minLat, double maxLat, double minLon, double maxLon);
    }
}
