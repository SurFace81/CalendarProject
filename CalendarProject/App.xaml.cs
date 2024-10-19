using CalendarProject.Activation;
using CalendarProject.Contracts.Services;
using CalendarProject.Core.Contracts.Services;
using CalendarProject.Core.Services;
using CalendarProject.EntityFramework;
using CalendarProject.Helpers;
using CalendarProject.Models;
using CalendarProject.Notifications;
using CalendarProject.Services;
using CalendarProject.ViewModels;
using CalendarProject.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace CalendarProject
{

    // To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
    public partial class App : Application
    {
        // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        public IHost Host
        {
            get;
        }

        public static T GetService<T>()
            where T : class
        {
            if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }

            return service;
        }

        public static WindowEx MainWindow { get; } = new MainWindow();

        public static UIElement? AppTitlebar { get; set; }

        public App()
        {
            InitializeComponent();

            Host = Microsoft.Extensions.Hosting.Host.
            CreateDefaultBuilder().
            UseContentRoot(AppContext.BaseDirectory).
            ConfigureServices((context, services) =>
            {
                // Default Activation Handler
                services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                // Other Activation Handlers
                services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

                // Services
                services.AddSingleton<IAppNotificationService, AppNotificationService>();
                services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddTransient<INavigationViewService, NavigationViewService>();

                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IFileService, FileService>();

                // Views and ViewModels
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsPage>();
                services.AddTransient<UserProfileViewModel>();
                services.AddTransient<UserProfilePage>();
                services.AddTransient<WeekViewModel>();
                services.AddTransient<WeekPage>();
                services.AddTransient<AddEventViewModel>();
                services.AddTransient<AddEventPage>();
                services.AddTransient<CalendarViewModel>();
                services.AddTransient<CalendarPage>();
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();

                // My classes
                services.AddSingleton<DbWorker>(provider =>
                {
                    var dbPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                        "CalendarProject", 
                        "app.db"
                    );
                    return new DbWorker(dbPath);
                });

                // Configuration
                services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            }).
            Build();

            App.GetService<IAppNotificationService>().Initialize();

            UnhandledException += App_UnhandledException;

#if DEBUG
            var dbWorker = App.GetService<DbWorker>();

            TestDbCreator test = new TestDbCreator();

            dbWorker.DbAdd<User>(test.User1, test.User2, test.User3);
            dbWorker.DbAdd<Event>(test.Event1, test.Event2, test.Event3, 
                                  test.Event4, test.Event5, test.Event6, 
                                  test.Event7, test.Event8, test.Event9,
                                  test.Event10);
            dbWorker.DbAdd<Settings>(test.Settings1, test.Settings2, test.Settings3);

            SessionContext.UserId = dbWorker.DbExecuteSQL<User>("SELECT * FROM Users WHERE AutoLogin = 1").First().Id;
#endif
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO: Log and handle exceptions as appropriate.
            // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            // Отправка уведомления при запуске
            //App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

#if DEBUG
            await App.GetService<IActivationService>().ActivateAsync(args);
#elif RELEASE        
            (new LoginWindow(args)).Activate();
#endif
        }
    }
}