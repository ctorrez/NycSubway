using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.Database.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedUserData(UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new IdentityUser()
                {
                    UserName = "ctorrez",
                    Email = "ctorrez@test.com",
                };
                await userManager.CreateAsync(user, "Challenge#2022");
            }
        }
    }
}
