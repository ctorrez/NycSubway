using Microsoft.Extensions.DependencyInjection;
using NycSubway.Core.Services.Identity;
using NycSubway.Core.Services.Station;
using NycSubway.Core.Services.User;
using NycSubway.Infrastructure.Identity;
using NycSubway.Infrastructure.Repositories;
using NycSubway.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NycSubway.WebApi.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStationRepo, StationRepo>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<StationService, StationService>();

            return services;
        }
    }
}
