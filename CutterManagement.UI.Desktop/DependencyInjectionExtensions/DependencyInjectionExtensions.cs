using CutterManagement.Core;
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
            services.AddSingleton<NavigationBarViewModel>();
            services.AddSingleton<ApplicationMainViewModel>();
            services.AddSingleton(provider => new HomePageViewModel(provider.GetRequiredService<MachineItemCollectionViewModel>()));
            services.AddSingleton(provider => new MachineItemCollectionViewModel(provider.GetRequiredService<IDataAccessService<MachineDataModel>>()));

            // Transients
            services.AddTransient<InfoPageViewModel>();
            services.AddTransient<UsersPageViewModel>();
            services.AddTransient<UpdatesPageViewModel>();
            services.AddTransient<ArchivesPageViewModel>();
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
            services.AddTransient<Func<AppPage, ViewModelBase>>(serviceProvider => pageProvider =>
            {
                switch(pageProvider)
                {
                    case AppPage.HomePage:
                        return serviceProvider.GetRequiredService<HomePageViewModel>();

                    case AppPage.UpdatePage:
                        return serviceProvider.GetRequiredService<UpdatesPageViewModel>();

                    case AppPage.ArchivePage:
                        return serviceProvider.GetRequiredService<ArchivesPageViewModel>();

                    case AppPage.UserPage:
                        return serviceProvider.GetRequiredService<UsersPageViewModel>();

                    case AppPage.SettingsPage:
                        return serviceProvider.GetRequiredService<SettingsPageViewModel>();

                    case AppPage.InformationPage:
                        return serviceProvider.GetRequiredService<InfoPageViewModel>();

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
