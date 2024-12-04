using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CutterManagement.DataAccess
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);

            services.AddTransient<IDataAccessService>(serviceProvider => new DataAccessService(serviceProvider.GetRequiredService<ApplicationDbContext>()));

            return services;
        }
    }
}
