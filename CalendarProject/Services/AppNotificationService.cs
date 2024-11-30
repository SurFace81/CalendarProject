using System.Collections.Specialized;
using System.Web;
using CalendarProject.Contracts.Services;
using Microsoft.Windows.AppNotifications;

namespace CalendarProject.Notifications
{

    public class AppNotificationService : IAppNotificationService
    {
        private readonly INavigationService _navigationService;

        public AppNotificationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        ~AppNotificationService()
        {
            Unregister();
        }

        public void Initialize()
        {
            AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
            AppNotificationManager.Default.Register();
        }

        public void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                App.MainWindow.Show();
                if (App._trayIcon != null)
                {
                    App._trayIcon.Visible = false;
                }
                Dictionary<string, string> arg = ParseArguments(args);
                App.MainWindow.ShowMessageDialogAsync(arg["descr"], arg["header"]);
                App.MainWindow.BringToFront();
            });
        }

        private Dictionary<string, string> ParseArguments(AppNotificationActivatedEventArgs args)
        {
            var res = new Dictionary<string, string>();

            foreach (var pair in args.Argument.Split(';'))
            {
                res.Add(pair.Split('=')[0], pair.Split('=')[1]);
            }

            return res;
        }

        public bool Show(string payload)
        {
            var appNotification = new AppNotification(payload);

            AppNotificationManager.Default.Show(appNotification);

            return appNotification.Id != 0;
        }

        public NameValueCollection ParseArguments(string arguments)
        {
            return HttpUtility.ParseQueryString(arguments);
        }

        public void Unregister()
        {
            AppNotificationManager.Default.Unregister();
        }
    }
}