using CalendarProject.Activation;
using CalendarProject.Contracts.Services;
using CalendarProject.Core.Contracts.Services;
using CalendarProject.Core.Services;
using CalendarProject.EntityFramework;
using CalendarProject.Models;
using CalendarProject.Notifications;
using CalendarProject.Services;
using CalendarProject.ViewModels;
using CalendarProject.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Windows.Globalization;
using System.Windows.Forms;

namespace CalendarProject
{
    public partial class App : Microsoft.UI.Xaml.Application
    {
        public IHost Host { get; }

        public static LaunchActivatedEventArgs? LaunchArgs { get; private set; }

        public static T GetService<T>()
            where T : class
        {
            if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }

            return service;
        }

        public static WindowEx MainWindow { get; set; }
        public static WindowEx loginWindow { get; set; }

        public static UIElement? AppTitlebar { get; set; }

        public static NotifyIcon? _trayIcon { get; set; }
        private ContextMenuStrip? _trayMenu;
        private bool isClosingApp = false;

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
                services.AddTransient<DayViewModel>();
                services.AddTransient<DayPage>();
                services.AddTransient<AddEventViewModel>();
                services.AddTransient<AddEventPage>();
                services.AddTransient<CalendarViewModel>();
                services.AddTransient<CalendarPage>();
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();
                services.AddTransient<LoginPage>();
                services.AddTransient<SignUpPage>();
                services.AddTransient<ForgotPage>();

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
                services.AddSingleton<BgNotificationService>();

                // Configuration
                services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            }).
            Build();

            App.GetService<IAppNotificationService>().Initialize();

            UnhandledException += App_UnhandledException;

            InitializeTray();

#if DEBUG
            var dbWorker = App.GetService<DbWorker>();

            TestDbCreator test = new TestDbCreator();

            dbWorker.DbAdd<User>(test.User1, test.User2, test.User3);
            dbWorker.DbAdd<Event>(test.Event1, test.Event2, test.Event3,
                                  test.Event4, test.Event5, test.Event6,
                                  test.Event7, test.Event8, test.Event9,
                                  test.Event10);
            dbWorker.DbAdd<Settings>(test.Settings1, test.Settings2, test.Settings3);

            SessionContext.CurrentUser = dbWorker.DbExecuteSQL<User>("SELECT * FROM Users").First();

            SessionContext.CurrentSettings = dbWorker.DbExecuteSQL<Settings>(
                "SELECT * FROM Settings WHERE UserId = @p0",
                SessionContext.CurrentUser.Id
            ).First();
            SessionContext.StartLangId = SessionContext.CurrentSettings.LangId;

            ApplicationLanguages.PrimaryLanguageOverride = SessionContext.StartLangId;  // ставим язык
#endif
            App.GetService<BgNotificationService>().Initialize();
            App.GetService<BgNotificationService>().Start();
        }

        private void InitializeTray()
        {
            _trayIcon = new System.Windows.Forms.NotifyIcon()
            {
                Icon = new System.Drawing.Icon(Path.Combine(AppContext.BaseDirectory, "Assets", "WindowIcon.ico")),
                Visible = false,
                Text = "CalendarProject"
            };

            _trayMenu = new ContextMenuStrip();
            _trayMenu.Items.Add("Показать");
            _trayMenu.Items.Add("Выход");
            _trayMenu.ItemClicked += (o, e) =>
            {
                _trayIcon.Visible = false;
                switch (e.ClickedItem?.Text)
                {
                    case "Показать":
                        MainWindow.Show();
                        MainWindow.Activate();
                        break;
                    case "Выход":
                        isClosingApp = true;
                        App.Current.Exit();
                        break;
                    default:
                        break;
                }
            };

            _trayIcon.ContextMenuStrip = _trayMenu;
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            LaunchArgs = args;

            MainWindow = new MainWindow();
            MainWindow.Closed += MainWindow_Closed;
#if DEBUG
            await App.GetService<IActivationService>().ActivateAsync(args);
#elif RELEASE
            loginWindow = new LoginWindow(args);
            loginWindow.Content = App.GetService<LoginPage>();
            loginWindow.Activate();
#endif
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            args.Handled = !isClosingApp;
            if (_trayIcon != null && !isClosingApp)
            {
                _trayIcon.Visible = true;
                MainWindow.Hide();
            }            
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }
    }
}