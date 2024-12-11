using Microsoft.Extensions.DependencyInjection;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Dependency injection extensions
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Inject view models into dependency service
        /// </summary>
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Add services

            // Singletons
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<NavigationBarViewModel>();
            services.AddSingleton<ApplicationMainViewModel>();
            services.AddSingleton<MachineItemCollectionViewModel>();

            // Transients
            services.AddTransient<SettingsPageViewModel>();
            services.AddTransient<ApplicationWindowViewModel>();
            
            // Return services
            return services;
        }

        /// <summary>
        /// Inject services into dependency service
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Add services
            services.AddSingleton<PageFactory>();
            services.AddSingleton<Func<AppPage, ViewModelBase>>(serviceProvider => pageProvider =>
            {
                switch(pageProvider)
                {
                    case AppPage.HomePage:
                        return serviceProvider.GetRequiredService<HomePageViewModel>();

                    case AppPage.SettingsPage:
                        return serviceProvider.GetRequiredService<SettingsPageViewModel>();

                    default:
                        throw new InvalidOperationException();
                }
            });

            // Return services
            return services;
        }



        /// <summary>
        /// Inject app views into dependency service
        /// </summary>
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            // Add services
            services.AddSingleton<MainWindow>();

            // Return services
            return services;
        }
    }
}
