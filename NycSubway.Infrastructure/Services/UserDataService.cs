using Microsoft.EntityFrameworkCore;
using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using NycSubway.Core.Services.User;
using NycSubway.Data.Entities;
using NycSubway.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NycSubway.Infrastructure.Services
{
    public class UserDataService : IUserDataService
    {
        private SubwayDbContext _context;

        public UserDataService(SubwayDbContext context)
        {
            _context = context;
        }

        public List<StationEntranceModel> GetStationEntrances(UserReadModel model)
        {
            var user = _context.AppUsers.First(x => x.UserName == model.UserName);

            var entrances = _context
                .AppUserStationEntrances
                .Include(x => x.StationEntrance)
                .Where(x => x.AppUserId == user.Id)
                .Select(x => x.StationEntrance)
                .ToList();

            return Project(entrances);
        }

        private List<StationEntranceModel> Project(List<StationEntrance> entrances)
        {
            var models = new List<StationEntranceModel>();

            foreach(var x in entrances)
            {
                models.Add(new StationEntranceModel()
                {
                    Name = x.Name,
                    Point = new GeoPoint()
                    {
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    }
                });
            }

            return models;
        }

        public UserReadModel GetUserReadModel(string email)
        {
            var user = _context.AppUsers.First(x => x.Email == email);

            return new UserReadModel()
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }

        public ResultResponse SaveStationEntrance(UserReadModel model, StationEntranceModel entranceModel)
        {
            var user = _context.AppUsers.First(x => x.UserName == model.UserName);

            var entrance = _context.StationEntrances.First(x => x.Name == entranceModel.Name);

            _context.AppUserStationEntrances.Add(new AppUserStationEntrance()
            {
                AppUserId = user.Id,
                StationEntranceId = entrance.Id,
            });

            _context.SaveChanges();

            return ResultResponse.SuccessResponse();
        }
    }
}
