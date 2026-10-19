using System.Collections.Generic;
using System.Threading.Tasks;

namespace NycSubway.Core.Services.Station
{
    public interface IStationRepo
    {
        Task<List<StationEntranceModel>> GetStationEntrancesAsync();
    }
}
