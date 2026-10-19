using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NycSubway.Core.Entities;

namespace NycSubway.Database.Configs
{
    public class AppUserStationEntranceConfig : IEntityTypeConfiguration<AppUserStationEntrance>
    {
        public void Configure(EntityTypeBuilder<AppUserStationEntrance> builder)
        {

        }
    }
}
