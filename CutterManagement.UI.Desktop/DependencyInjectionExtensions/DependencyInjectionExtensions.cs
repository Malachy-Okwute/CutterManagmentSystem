using CutterManagement.Core;
using CutterManagement.DataAccess;
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
            // Add view models

            // Singletons
            // NOTE: Does not allow migration
            //services.AddSingleton<HomePageViewModel>();
            //services.AddSingleton<NavigationBarViewModel>();
            //services.AddSingleton<MachineItemCollectionViewModel>();

            // Singletons 
            // NOTE: Allows migration
            services.AddSingleton(provider => 
            new NavigationBarViewModel(provider.GetRequiredService<PageFactory>(), provider.GetRequiredService<ShiftProfileViewModel>()));
            services.AddSingleton(provider => new HomePageViewModel(provider.GetRequiredService<MachineItemCollectionViewModel>()));
            services.AddSingleton(provider => new MachineItemCollectionViewModel(provider.GetRequiredService<IMachineService>()));

            // Transients
            services.AddTransient<InfoPageViewModel>();
            services.AddTransient<UsersPageViewModel>();
            services.AddTransient<UpdatesPageViewModel>();
            services.AddTransient<ArchivesPageViewModel>();
            services.AddTransient<SettingsPageViewModel>();
            services.AddTransient<ShiftProfileViewModel>();
            services.AddTransient<ApplicationWindowViewModel>();

            // Dialog view model
            services.AddTransient<CMMCheckDialogViewModel>();
            services.AddTransient<CutterSwapDialogViewModel>();
            services.AddTransient<MachineSetupDialogViewModel>();
            services.AddTransient<NewInfoUpdateDialogViewModel>();
            services.AddTransient<CutterRemovalDialogViewModel>();
            services.AddTransient<FrequencyCheckDialogViewModel>();
            services.AddTransient<CutterRelocationDialogViewModel>();
            services.AddTransient<MachineConfigurationDialogViewModel>();
            services.AddTransient<MachineStatusSettingDialogViewModel>();
            
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

            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IDialogViewModelFactory, DialogViewModelFactory>();
            services.AddScoped<IDataAccessServiceFactory, DataAccessServiceFactory>();
            //services.AddScoped(typeof(IDataAccessService<>), typeof(DataAccessService<>));

            // Register dialog service
            DialogService.RegisterDialog<CMMCheckDialogViewModel, CMMCheckDialog>();
            DialogService.RegisterDialog<AdminLoginDialogViewModel, AdminLoginDialog>();
            DialogService.RegisterDialog<CutterSwapDialogViewModel, CutterSwapDialog>();
            DialogService.RegisterDialog<CreatePartDialogViewModel, CreatePartDialog>();
            DialogService.RegisterDialog<CreateUserDialogViewModel, CreateUserDialog>();
            DialogService.RegisterDialog<UserManagerDialogViewModel, UserManagerDialog>();
            DialogService.RegisterDialog<MachineSetupDialogViewModel, MachineSetupDialog>();
            DialogService.RegisterDialog<CutterRemovalDialogViewModel, CutterRemovalDialog>();
            DialogService.RegisterDialog<NewInfoUpdateDialogViewModel, NewInfoUpdateDialog>();
            DialogService.RegisterDialog<FrequencyCheckDialogViewModel, FrequencyCheckDialog>();
            DialogService.RegisterDialog<CutterRelocationDialogViewModel, CutterRelocationDialog>();
            DialogService.RegisterDialog<MachineStatusSettingDialogViewModel, StatusSettingDialog>();
            DialogService.RegisterDialog<MachineConfigurationDialogViewModel, MachineConfigurationDialog>();

            // Register data validation policy
            DataValidationService.RegisterValidationPolicy(new UserValidationPolicy());
            DataValidationService.RegisterValidationPolicy(new PartValidationPolicy());
            DataValidationService.RegisterValidationPolicy(new MachineValidationPolicy());

            // Return services
            return services;
        }

        /// <summary>
        /// Inject app views into dependency service
        /// </summary>
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            // Add views
            services.AddSingleton<MainWindow>();

            // Return services
            return services;
        }
    }
}
