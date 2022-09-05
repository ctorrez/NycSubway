using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NycSubway.Data.Entities;

namespace NycSubway.Database.Configs
{
    public class AppUserStationEntranceConfig : IEntityTypeConfiguration<AppUserStationEntrance>
    {
        public void Configure(EntityTypeBuilder<AppUserStationEntrance> builder)
        {

        }
    }
}
