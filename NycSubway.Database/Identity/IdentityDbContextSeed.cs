using Microsoft.AspNetCore.Identity;
using NycSubway.Database.Context;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.Database.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedUserData(UserManager<IdentityUser> userManager, SubwayDbContext context)
        {
            if (!userManager.Users.Any())
            {
                var user = new IdentityUser()
                {
                    UserName = "ctorrez",
                    Email = "ctorrez@test.com",
                };
                await userManager.CreateAsync(user, "Challenge#2022");

                await context.AppUsers.AddAsync(new Data.Entities.AppUser()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = "Christian",
                    LastName = "Torrez"
                });

                context.SaveChanges();
            }
        }
    }
}
