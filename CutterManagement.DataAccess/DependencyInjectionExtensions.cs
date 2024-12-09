using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CutterManagement.DataAccess
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddUserDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>(option =>
            {
                option.UseSqlite(configuration.GetConnectionString("UserDbConnection"));
            },ServiceLifetime.Transient);

            services.AddTransient<IUserDataAccessService>(serviceProvider => new UserDataAccessService(serviceProvider.GetRequiredService<UsersDbContext>()));

            return services;
        }
    }
}
