using CutterManagement.Core;
using CutterManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.IO;
using System.Windows;

namespace CutterManagement.UI.Desktop
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
        private static string _environment = string.Empty;

        /// <summary>
        /// The splash window for this application
        /// </summary>
        private readonly SplashWindow _splashWindow;

        #endregion

        #region Public Properties

        /// <summary>
        /// Application services host
        /// </summary>
        public IHost ApplicationHost { get; private set; } = default!;

        #endregion

        #region Default Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public App()
        {
            // Create splash window
            _splashWindow = new SplashWindow();
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

                        // Set up dependency injection service
                        ApplicationHost = CreateHostBuilder().Build();

                        // Log application start up as information 
                        Log.Logger.Information("Application is starting...");

                        // Finalizing...
                        await Task.Delay(TimeSpan.FromSeconds(6));
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

            // Update database migration or generate a database if not created.
            await ApplicationHost.Services.GetRequiredService<IDataAccessService>().UpdateDatabaseMigrateAsync();

            // Lunch main application window
            await LunchApplicationWindowAsync();

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
            await ApplicationHost.StopAsync();

            // Dispose on exit
            ApplicationHost.Dispose();

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
            await ApplicationHost.StartAsync();

            // Get application window from application host
            MainWindow = ApplicationHost.Services.GetRequiredService<MainWindow>();

            // Make sure we have window
            ArgumentNullException.ThrowIfNull(nameof(MainWindow));

            // TODO: Find a better/smarter way to implement this
            // Fade window out of view, then close it
            await Animations.FadeElementOutOfView(_splashWindow);

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
        /// Sets up serilog logger ready for use
        /// </summary>
        /// <param name="configurationBuilder">The logger configuration settings</param>
        private static void SetupSerilogLogger(IConfigurationBuilder configurationBuilder)
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
        /// </summary>
        private static IHostBuilder CreateHostBuilder(string[]? args = null)
        {
            // Setup services 
            return Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration(configurationBuilder =>
                 {
                     configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                         .AddJsonFile($"appsettings.{_environment}.json", optional: true);

                     SetupSerilogLogger(configurationBuilder);
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     // To apply ef core migration, the project configuring services (e.g. Dependency Injection)
                     // Have to do the following for migration to work
                     //
                     // - Define a static CreateHostBuilder(string[]? args) method and use it to setup services
                     //         (Ef core looks for a method with such signature during migration to locate Dbcontext type and db provider e.g. SqlServer).
                     // - AddDbContext to services and set it up.
                     // - Install ef core design and ef core tools nuget packages.
                     // - Use the project to run migrations by setting it as a startup project.
                     //
                     // **** Use -verbose to see what's actually going on during migration. ****

                     services.AddDbContext<ApplicationDbContext>(option =>
                     {
                         option.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultDbConnection")!
                                                                      // https://learn.microsoft.com/en-us/answers/questions/1113995/changing-location-of-database-mdf-file-from-defaul
                                                                      // Create the *.mfd file in the bin folder instead of the user folder
                                                                      .Replace("[DataDirectory]", Directory.GetCurrentDirectory()));
                     });
                     services.AddTransient<IDataAccessService>(serviceProvider => new DataAccessService(serviceProvider.GetRequiredService<ApplicationDbContext>()));

                     services.AddViewModels();
                     services.AddServices();
                     services.AddViews();
                 })
                 .UseSerilog();
        }

        #endregion
    }
}
