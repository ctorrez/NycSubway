using NycSubway.Core.Entities;
using NycSubway.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NycSubway.Core.Services.Station
{
    public interface IStationRepo
    {
        Task<List<StationEntranceModel>> GetStationEntrancesAsync();
        Task<PagedResult<StationEntranceModel>> GetStationEntrancesAsync(string search, int page, int pageSize);
        Task<List<StationEntrance>> GetStationEntrancesInBoundingBoxAsync(
            double minLat, double maxLat, double minLon, double maxLon);
    }
}
