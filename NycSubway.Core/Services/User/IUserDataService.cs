using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using System.Collections.Generic;

namespace NycSubway.Core.Services.User
{
    public interface IUserDataService
    {
        UserReadModel GetUserReadModel(string email);
        List<StationEntrance> GetStationEntrances(UserReadModel model);
        ResultResponse SaveStationEntrance(UserReadModel model, StationEntrance entrance);
    }
}
