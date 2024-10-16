﻿using CalendarProject.Activation;
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

                // Configuration
                services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            }).
            Build();

            App.GetService<IAppNotificationService>().Initialize();

            UnhandledException += App_UnhandledException;

#if TEST
            DbWorker dbWorker = new DbWorker(Path.Combine(AppContext.BaseDirectory, "Assets/app.db"));

            User us1 = new User { Name = "us123", Email = "123@321" };
            User us2 = new User { Name = "usABC", Email = "abc@321" };

            Event ev1 = new Event { Header = "ev1", Description = "descr1", Time = DateTime.Now, Priority = 1, User = us1 };
            Event ev2 = new Event { Header = "ev2", Description = "descr2", Time = DateTime.Now, Priority = 2, User = us2 };
            Event ev3 = new Event { Header = "ev3", Description = "descr3", Time = DateTime.Now, Priority = 3, User = us1 };

            Settings sett = new Settings { User = us1, ThemeId = 1, LangId = 1 };

            dbWorker.DbAdd<User>(us1, us2);
            dbWorker.DbAdd<Event>(ev1, ev2, ev3);
            dbWorker.DbAdd<Settings>(sett);
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

            await App.GetService<IActivationService>().ActivateAsync(args);
        }
    }
}