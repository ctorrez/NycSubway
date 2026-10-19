using Microsoft.EntityFrameworkCore;
using NycSubway.Core.Services.Station;
using NycSubway.Database.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.Infrastructure.Repositories
{
    public class StationRepo : IStationRepo
    {
        private readonly SubwayDbContext _context;

        public StationRepo(SubwayDbContext context)
        {
            _context = context;
        }

        public async Task<List<StationEntranceModel>> GetStationEntrancesAsync()
        {
            return await _context.StationEntrances
                .AsNoTracking()
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
        }
    }
}
