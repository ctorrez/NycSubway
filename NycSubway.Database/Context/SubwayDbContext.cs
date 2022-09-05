using Microsoft.EntityFrameworkCore;
using NycSubway.Data.Entities;
using NycSubway.Database.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NycSubway.Database.Context
{
    public class SubwayDbContext : DbContext
    {
        public SubwayDbContext(DbContextOptions<SubwayDbContext> options) : base(options)
        {

        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserStationEntrance> AppUserStationEntrances { get; set; }
        public virtual DbSet<StationEntrance> StationEntrances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserConfig());
            modelBuilder.ApplyConfiguration(new AppUserStationEntranceConfig());
            modelBuilder.ApplyConfiguration(new StationEntranceConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
