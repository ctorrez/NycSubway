using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NycSubway.Data.Entities;

namespace NycSubway.Database.Configs
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(256);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(256);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Email).HasMaxLength(256);

            builder.HasIndex(x => new { x.UserName }).IsUnique();
            builder.HasIndex(x => new { x.FirstName, x.LastName });
            builder.HasIndex(x => new { x.Email });
        }
    }
}
