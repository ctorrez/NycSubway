using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NycSubway.Core.Services.User
{
    public interface IUserDataService
    {
        Task<UserReadModel> GetUserReadModelAsync(string email);
        Task<List<StationEntranceModel>> GetStationEntrancesAsync(UserReadModel model);
        Task<ResultResponse> SaveStationEntranceAsync(UserReadModel model, StationEntranceModel entranceModel);
    }
}
