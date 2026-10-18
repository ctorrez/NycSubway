using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NycSubway.Database.Identity;
using System.Text;

namespace NycSubway.WebApi.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,
            IConfiguration _configuration)
        {
            services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddSignInManager<SignInManager<IdentityUser>>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                            ValidIssuer = _configuration["Token:Issuer"],
                            ValidateIssuer = true,
                            ValidateAudience = false
                        };
                    }
                );

            return services;
        }
    }
}
