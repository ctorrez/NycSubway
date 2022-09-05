using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NycSubway.Data.Entities;

namespace NycSubway.Database.Configs
{
    public class StationEntranceConfig : IEntityTypeConfiguration<StationEntrance>
    {
        public void Configure(EntityTypeBuilder<StationEntrance> builder)
        {
            builder.HasIndex(x => new { x.Name }).IsClustered(false);
        }
    }
}
