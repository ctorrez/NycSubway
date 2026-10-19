using Microsoft.EntityFrameworkCore;
using NycSubway.Core.Entities;
using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using NycSubway.Core.Services.User;
using NycSubway.Database.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.Infrastructure.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly SubwayDbContext _context;

        public UserDataService(SubwayDbContext context)
        {
            _context = context;
        }

        public async Task<UserReadModel> GetUserReadModelAsync(string email)
        {
            var user = await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            return new UserReadModel
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }

        public async Task<List<StationEntranceModel>> GetStationEntrancesAsync(UserReadModel model)
        {
            if (model == null)
            {
                return new List<StationEntranceModel>();
            }

            var user = await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == model.UserName);

            if (user == null)
            {
                return new List<StationEntranceModel>();
            }

            var entrances = await _context.AppUserStationEntrances
                .AsNoTracking()
                .Where(x => x.AppUserId == user.Id)
                .Select(x => x.StationEntrance)
                .Select(x => new StationEntranceModel
                {
                    Name = x.Name,
                    Point = new GeoPoint
                    {
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    }
                })
                .ToListAsync();

            return entrances;
        }

        public async Task<ResultResponse> SaveStationEntranceAsync(UserReadModel model, StationEntranceModel entranceModel)
        {
            if (model == null || entranceModel == null || string.IsNullOrWhiteSpace(entranceModel.Name))
            {
                return ResultResponse.FailureResponse("Invalid request data.");
            }

            var user = await _context.AppUsers
                .FirstOrDefaultAsync(x => x.UserName == model.UserName);

            if (user == null)
            {
                return ResultResponse.FailureResponse("User not found.");
            }

            var entrance = await _context.StationEntrances
                .FirstOrDefaultAsync(x => x.Name == entranceModel.Name);

            if (entrance == null)
            {
                return ResultResponse.FailureResponse($"Station entrance '{entranceModel.Name}' was not found.");
            }

            var alreadySaved = await _context.AppUserStationEntrances
                .AnyAsync(x => x.AppUserId == user.Id && x.StationEntranceId == entrance.Id);

            if (alreadySaved)
            {
                return ResultResponse.SuccessResponse("Station entrance is already saved.");
            }

            await _context.AppUserStationEntrances.AddAsync(new AppUserStationEntrance
            {
                AppUserId = user.Id,
                StationEntranceId = entrance.Id
            });

            await _context.SaveChangesAsync();

            return ResultResponse.SuccessResponse("Station entrance saved successfully.");
        }
    }
}
