using CutterManagement.Core;
using CutterManagement.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            services.AddSingleton<NavigationBarViewModel>();
            services.AddSingleton(provider => new HomePageViewModel(provider.GetRequiredService<MachineItemCollectionViewModel>()));
            services.AddSingleton(provider => new MachineItemCollectionViewModel(provider.GetRequiredService<IMachineService>()));

            // Transients
            services.AddTransient<InfoPageViewModel>();
            services.AddTransient<UpdatesPageViewModel>();
            services.AddTransient<ArchivesPageViewModel>();
            services.AddTransient<SettingsPageViewModel>();
            services.AddTransient<ApplicationWindowViewModel>();
            services.AddTransient(provider => new UsersPageViewModel(provider.GetRequiredService<IDataAccessServiceFactory>()));

            // Dialog view model
            services.AddTransient(provider => new CMMCheckDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new CutterSwapDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new MachineSetupDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new CutterRemovalDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new FrequencyCheckDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new CutterRelocationDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new MachineConfigurationDialogViewModel(provider.GetRequiredService<IMachineService>()));
            services.AddTransient(provider => new MachineStatusSettingDialogViewModel(provider.GetRequiredService<IMachineService>()));

            
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

            services.AddScoped<IDataAccessServiceFactory, DataAccessServiceFactory>();
            //services.AddScoped(typeof(IDataAccessService<>), typeof(DataAccessService<>));
            services.AddTransient<IDialogViewModelFactory, DialogViewModelFactory>(provider => new DialogViewModelFactory(provider.GetRequiredService<IServiceProvider>()));
            services.AddTransient<IMachineService, MachineService>(provider
                => new MachineService(provider.GetRequiredService<IDataAccessServiceFactory>(), provider.GetRequiredService<IDialogViewModelFactory>()));

            // Register dialog service
            DialogService.RegisterDialog<CMMCheckDialogViewModel, CMMCheckDialog>();
            DialogService.RegisterDialog<AdminLoginDialogViewModel, AdminLoginDialog>();
            DialogService.RegisterDialog<CutterSwapDialogViewModel, CutterSwapDialog>();
            DialogService.RegisterDialog<CreatePartDialogViewModel, CreatePartDialog>();
            DialogService.RegisterDialog<CreateUserDialogViewModel, CreateUserDialog>();
            DialogService.RegisterDialog<UserManagerDialogViewModel, UserManagerDialog>();
            DialogService.RegisterDialog<MachineSetupDialogViewModel, MachineSetupDialog>();
            DialogService.RegisterDialog<CutterRemovalDialogViewModel, CutterRemovalDialog>();
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
