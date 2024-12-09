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
            services.AddSingleton<ApplicationWindowViewModel>();
            services.AddSingleton<MachineItemCollectionViewModel>();
            
            // Return services
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<PageFactory>();
            services.AddSingleton<Func<AppPage, PagePresenterViewModel>>(serviceProvider => pageProvider =>
            {
                var currentPage = serviceProvider.GetRequiredService<PagePresenterViewModel>();
                switch(pageProvider)
                {
                }

                return currentPage;
            });

            // Return services
            return services;
        }



        /// <summary>
        /// Inject app views into dependency service
        /// </summary>
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            // Return services
            return services;
        }
    }
}
