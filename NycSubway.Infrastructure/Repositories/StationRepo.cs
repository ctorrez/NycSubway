using Microsoft.EntityFrameworkCore;
using NycSubway.Core.Entities;
using NycSubway.Core.Models;
using NycSubway.Core.Services.Station;
using NycSubway.Database.Context;
using System;
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

        public async Task<PagedResult<StationEntranceModel>> GetStationEntrancesAsync(string search, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _context.StationEntrances.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{trimmed}%"));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            return new PagedResult<StationEntranceModel>(items, totalCount, page, pageSize);
        }

        public async Task<List<StationEntrance>> GetStationEntrancesInBoundingBoxAsync(GeoBoundingBox box)
        {
            return await _context.StationEntrances
                .AsNoTracking()
                .Where(x => x.Latitude >= box.MinLatitude && x.Latitude <= box.MaxLatitude &&
                            x.Longitude >= box.MinLongitude && x.Longitude <= box.MaxLongitude)
                .ToListAsync();
        }
    }
}
