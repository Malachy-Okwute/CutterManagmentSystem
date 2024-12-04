using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.DataAccess
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CutterManagementSystemDatabase;Trusted_Connection=True;");
            //}, ServiceLifetime.Transient);

            services.AddTransient<IDataAccessService>(x => new DataAccessService(x.GetRequiredService<ApplicationDbContext>()));

            return services;
        }
    }
}
