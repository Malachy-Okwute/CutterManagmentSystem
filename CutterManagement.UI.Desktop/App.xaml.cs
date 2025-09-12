using CutterManagement.Core;
using CutterManagement.DataAccess;
using Microsoft.Data.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using Serilog;
using Serilog.Events;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        #region Constructor

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

                        //using (HttpClient client = new HttpClient())
                        //{
                        //    HttpResponseMessage response = await client.GetAsync($"https://localhost:7057/userdatamodel");
                        //    response.EnsureSuccessStatusCode(); // Throws an exception if not successful

                        //    string jsonResponse = await response.Content.ReadAsStringAsync();

                        //    var test = JsonSerializer.Deserialize<List<UserDataModel>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        //}                        

                        #region Old Code
                        
                        //var appDbContext = ApplicationHost.Services.GetRequiredService<ApplicationDbContext>();

                        //// Make sure we have a database
                        //if ((await appDbContext.Database.CanConnectAsync()) is false)
                        //{
                        //    Dispatcher.Invoke(() =>
                        //    {
                        //        // Message
                        //        string message = $"Goto: \"Users > Public > Public Documents > CutterManagementSystem > DatabaseServerName.txt\" " +
                        //        $"and provide a valid sql server details in this format server-name;user-id;password; for the application.{Environment.NewLine} {Environment.NewLine}" +
                        //        $"NOTE: Save the details provided to the prior to running the application.";

                        //        // Dialog box configuration
                        //        var result = MessageBox.Show(message, "Database error", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);

                        //        // Shut down application
                        //        if (result == MessageBoxResult.OK)
                        //        {
                        //            // Mark task as completed
                        //            taskCompletionSource.TrySetResult(true);

                        //            // try closing the splash window on ui thread
                        //            _splashWindow.Close();

                        //            // Close application 
                        //            Application.Current.Shutdown();
                        //        }
                        //    });
                        //}

                        #endregion

                        // Log application start up as information 
                        Log.Logger.Information("Application is starting...");

                        // Finalizing...
                        //await Task.Delay(TimeSpan.FromSeconds(4));
                    }
                    // If there is an error...
                    catch (Exception ex)
                    {
                        // Log error
                        Log.Logger.Information($"Error occurred. {Environment.NewLine} {ex.GetBaseException().Message}");

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

            //// Get database 
            //ApplicationDbContext db = ApplicationHost.Services.GetRequiredService<ApplicationDbContext>();

            //// Log 
            //Log.Logger.Information($"Attempting to apply database migration...");

            //// Update database migration or generate a database if not created.
            //await db.UpdateDatabaseMigrateAsync();

            // If admin doesn't exist...
            //if (await db.Users.AnyAsync(user => user.LastName == "admin") is false)
            //{
            //    // add admin user
            //    await db.Users.AddAsync(new UserDataModel
            //    {
            //        FirstName = "resource",
            //        LastName = "admin",
            //        DateCreated = DateTime.Now,
            //        Shift = UserShift.First
            //    });

            //    // Save changes
            //    await db.SaveChangesAsync();
            //}

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

            var shift = ApplicationHost.Services.GetRequiredService<ShiftProfileViewModel>();

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
            //ArgumentNullException.ThrowIfNull(nameof(MainWindow));

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
        /// <returns><see cref="string"/> as environment variable</returns>
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
            #region Old Code

            //// TEMPORARY FIX FOR DATABASE UNTIL A SERVER IS IMPLEMENTED TO HANDLE DATABASE SIDE OF THINGS
            //// REMOVE ONCE SERVER IS UP AND RUNNING
            //string[]? serverDetails = Array.Empty<string>();
            //var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CutterManagementSystem");
            //var localDbDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "CutterManagementSystem");

            //if (File.Exists(Path.Combine(localDbDir, "DatabaseServerName.txt")) is false)
            //{
            //    // Create local db folder
            //    Directory.CreateDirectory(localDbDir);

            //    var dbServerNamePath = Path.Combine(localDbDir, "DatabaseServerName.txt");

            //    File.WriteAllText(dbServerNamePath, "ReplaceThisWithYourServerName;ReplaceThisWithYourServerUserId;ReplaceThisWithYourServerPassword;");
            //}

            //// Read the file
            //var fileContent = File.ReadAllText(Path.Combine(localDbDir, "DatabaseServerName.txt"));

            //if (string.IsNullOrEmpty(fileContent) is false)
            //{
            //    serverDetails = fileContent.Trim().Replace("\\\\", "\\").Split(";");
            //}

            //// Define appsettings 
            //var appSettings = new
            //{
            //    Serilog = new
            //    {
            //        MinimumLevel = new
            //        {
            //            Default = "Information",
            //            Override = new
            //            {
            //                Microsoft = "Information",
            //                System = "Warning",
            //            }
            //        }
            //    },

            //    //ConnectionStrings = new { LocalDbConnection = "Server=ServerName;Database=DatabaseName;User Id=dev;Password=devenv;TrustServerCertificate=True;" }
            //    //ConnectionStrings = new { LocalDbConnection = "Server=MAL-DEV-ENVIRONMENT;Database=CutterManagementSystemDatabase;User Id=dev;Password=devenv;TrustServerCertificate=True;" }
            //    //ConnectionStrings = new { LocalDbConnection = "Server=(localdb)\\MSSQLLocalDB;Database=CutterManagementSystemDatabase;Trusted_Connection=True;" }
            //    ConnectionStrings = new { LocalDbConnection = $"Server={serverDetails[0]};User Id={serverDetails[1]};Password={serverDetails[2]};Database=CutterManagementSystemDatabase;Trusted_Connection=True;TrustServerCertificate=True;" }
            //};

            //// Serialize appsettings
            //var json = JsonSerializer.Serialize(appSettings, new JsonSerializerOptions { WriteIndented = true });

            //// Create a folder
            //Directory.CreateDirectory(configDir);

            //var configPath = Path.Combine(configDir, "appsettings.json");

            //// Create json file
            //File.WriteAllText(configPath, json); 

            #endregion

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
                         // TODO: Consider internet connection when using remote database
                         option.UseSqlServer(hostContext.Configuration.GetConnectionString("LocalDbConnection")!
                                                                    // https://learn.microsoft.com/en-us/answers/questions/1113995/changing-location-of-database-mdf-file-from-defaul
                                                                    // Create the *.mfd file in the bin folder instead of the user folder
                                                                    .Replace("[DataDirectory]", Directory.GetCurrentDirectory()));
                     }, ServiceLifetime.Scoped);

                     services.AddViewModels();
                     services.AddServices();
                     services.AddViews();
                 })
                 .UseSerilog();
        }

        #endregion
    }
}