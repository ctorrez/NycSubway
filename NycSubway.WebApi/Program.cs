using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NycSubway.Database.Context;
using NycSubway.Database.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var identityContext = services.GetRequiredService<IdentityContext>();
                    var subwayContext = services.GetRequiredService<SubwayDbContext>();

                    if (identityContext.Database.IsSqlite())
                    {
                        await identityContext.Database.EnsureCreatedAsync();
                    }
                    else
                    {
                        await identityContext.Database.MigrateAsync();
                    }

                    if (subwayContext.Database.IsSqlite())
                    {
                        await subwayContext.Database.EnsureCreatedAsync();
                    }
                    else
                    {
                        await subwayContext.Database.MigrateAsync();
                    }

                    await IdentityDbContextSeed.SeedUserData(userManager, subwayContext);
                    SubwayDbContextSeed.SeedNycData(subwayContext);
                    logger.LogInformation("Database initialized and seeded successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during database initialization or seeding.");
                    throw;
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
