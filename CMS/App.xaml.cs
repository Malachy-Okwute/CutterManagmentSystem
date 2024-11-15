using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Fields

        /// <summary>
        /// Environment variable (pulled using compiler directives)
        /// </summary>
        private string _environment = string.Empty;

        /// <summary>
        /// The splash window for this application
        /// </summary>
        private CMSSplashWindow _splashWindow { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Application services host
        /// </summary>
        public static IHost? ApplicationHost { get; private set; }

        #endregion

        #region Default Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public App()
        {
            // Create splash window
            _splashWindow = new CMSSplashWindow();
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Event that runs during application initial start-up
        /// </summary>
        /// <param name="e">Application startup event args</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Load application splash window on start up
            await LoadSplashWindowAsync(async (taskCompletionSource) =>
            {
                // make sure we are not on UI Thread 
                if (Thread.CurrentThread != Dispatcher.Thread)
                {
                    try
                    {
                        // Get environment variable
                        GetEnvironmentVariable();
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        // Configure application 
                        IConfigurationBuilder configBuilder = SetupConfigurationBuilder();
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        // Set up logger for the application 
                        SetupSerilogLogger(configBuilder);
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        // Log application start up as information 
                        Log.Logger.Information("Application is starting...");
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        // Set up dependency injection service
                        DependencyInjectionSetup();
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        //TODO: Get user settings and set application preferences such as theme etc
                        //TODO: Get current machine theme mode

                        await Task.Delay(TimeSpan.FromSeconds(1));
                        // Finalizing...
                    }
                    // If there is an error...
                    catch (Exception ex)
                    {
                        // Log the error 
                        Log.Logger.Warning(ex.Message);
                    }
                    finally
                    {
                        // Mark task as completed
                        taskCompletionSource.TrySetResult(true);
                    }
                }
                // Send task information
                await taskCompletionSource.Task;
            });

            // Lunch main application window
            await LunchApplicationWindowAsync();

            #region Old code

            //// Start the application host
            //await ApplicationHost!.StartAsync();

            //// Get application window from application host
            //MainWindow = ApplicationHost.Services.GetRequiredService<MainWindow>();

            ////// Make sure we have window
            ////ArgumentNullException.ThrowIfNull(nameof(MainWindow));

            //// Display window
            //MainWindow.Show();

            #endregion

            // Let base do what it needs
            base.OnStartup(e);
        }

        /// <summary>
        /// Event that runs when application is closing
        /// </summary>
        /// <param name="e">Application closing event args</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            // Log information
            Log.Logger.Information("Application is shutting down...");

            // Stop application host
            await ApplicationHost!.StopAsync();

            // Let base do what it needs
            base.OnExit(e);
        }

        #endregion

        #region Application Splash And Main Window

        /// <summary>
        /// Loads splash window into view as soon as application starts
        /// </summary>
        /// <param name="applicationSetup">Function to run in the background to set up 
        /// application services while showing the splash window </param>
        /// <returns><see cref="TaskCompletionSource"/> result</returns>
        private async Task<bool> LoadSplashWindowAsync(Func<TaskCompletionSource<bool>, Task> applicationSetup)
        {
            // Create a task completion object
            TaskCompletionSource<bool> taskResult = new TaskCompletionSource<bool>();

            // Show window
            _splashWindow.Show();

            await Task.Run( async delegate
            {
                // Make sure we're not on UI thread
                if(Thread.CurrentThread != Dispatcher.Thread)
                    // Set up application services
                    await applicationSetup.Invoke(taskResult);
            });

            // Return task result
            return taskResult.Task.Result;
        }

        /// <summary>
        /// Lunch application main window once app setup is done
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task LunchApplicationWindowAsync()
        {
            // Start the application host
            await ApplicationHost!.StartAsync();

            // Get application window from application host
            MainWindow = ApplicationHost.Services.GetRequiredService<MainWindow>();

            // Make sure we have window
            ArgumentNullException.ThrowIfNull(nameof(MainWindow));

            // TODO: Find a better way to implement this
            // Animate splash window
            Animations.Fade(_splashWindow, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 3, 0.6, 0, 1);
            await Task.Delay(TimeSpan.FromSeconds(0.6));

            // try closing the splash window on ui thread
            Dispatcher.Invoke(_splashWindow.Close);

            // Show main window
            MainWindow.Show();
        }

        #endregion

        #region Application Services Setup

        /// <summary>
        /// Provides environment variable indicating if we are running in 
        /// development or production environment
        /// </summary>
        /// <returns>Environment variable as a <see cref="string"/></returns>
        private string GetEnvironmentVariable()
        {
            #region Environment variable

            // Using compiler directives to determine when we are 
            // in DEVELOPMENT or PRODUCTION
#if DEBUG
            return _environment = "DEVELOPMENT";
#else
            return _environment = "PRODUCTION";
#endif

            #endregion
        }

        /// <summary>
        /// Configures application to enable the use of appsettings.json file
        /// </summary>
        /// <returns><see cref="IConfigurationBuilder"/></returns>
        private IConfigurationBuilder SetupConfigurationBuilder()
        {
            // Application configuration. Sets up .json
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{_environment}.json", optional: true);
                                //.AddEnvironmentVariables();

            return configurationBuilder;
        }

        /// <summary>
        /// Sets up serilog logger ready for use
        /// </summary>
        /// <param name="configurationBuilder">The logger configuration settings</param>
        private void SetupSerilogLogger(IConfigurationBuilder configurationBuilder)
        {
            // Configure serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationBuilder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File("app-log.txt", outputTemplate: "{Timestamp:MM-dd-yyyy hh:mm tt} {Level:u3} {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Information() 
                .MinimumLevel.Override("Default", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                .MinimumLevel.Override("System", LogEventLevel.Fatal)
                .CreateLogger();
        }

        /// <summary>
        /// Set up application dependency injection service
        /// SERVICES SETUP: Dependency Injection service
        ///                 Serilog
        /// </summary>
        private void DependencyInjectionSetup()
        {
            // Setup services 
            ApplicationHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddViewModels();
                    services.AddViews();
                })
                .UseSerilog()
                .Build();
        }

        #endregion
    }
}
