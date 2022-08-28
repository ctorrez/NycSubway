using Microsoft.Extensions.DependencyInjection;
using NycSubway.Core.Services.Station;
using NycSubway.Infrastructure.Repositories;
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
            services.AddScoped<StationService, StationService>();

            return services;
        }
    }
}
