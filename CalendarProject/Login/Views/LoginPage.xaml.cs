using CalendarProject.Contracts.Services;
using CalendarProject.EntityFramework;
using CalendarProject.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Globalization;

namespace CalendarProject.Views
{
    public sealed partial class LoginPage : Page
    {
        private DbWorker worker;
        private BgNotificationService notificationService;

        public LoginPage()
        {
            this.InitializeComponent();
            passw_err.Visibility = Visibility.Collapsed;
            email_err.Visibility = Visibility.Collapsed;

            worker = App.GetService<DbWorker>();
            notificationService = App.GetService<BgNotificationService>();
        }

        private void login_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login();
                e.Handled = true;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private async void Login()
        {
            if (SessionContext.ValidatePassword(passwText.Password) && SessionContext.ValidateEmail(emailText.Text))
            {
                if (UserIsExist(SessionContext.GetMD5Hash(passwText.Password), emailText.Text))
                {
                    emailText.ClearValue(Control.StyleProperty);
                    passwText.ClearValue(Control.StyleProperty);

                    notificationService.Initialize();
                    notificationService.Start();

                    await App.GetService<IActivationService>().ActivateAsync(LoginWindow.args);

                    App.loginWindow.Close();
                }
                else
                {
                    emailText.Style = (Style)Resources["ErrorTextBoxStyle"];
                    passwText.Style = (Style)Resources["ErrorPasswordBoxStyle"];
                }
            }
            else
            {
                passw_err.Visibility = Visibility.Collapsed;
                email_err.Visibility = Visibility.Collapsed;

                if (!SessionContext.ValidatePassword(passwText.Password))
                {
                    passw_err.Visibility = Visibility.Visible;
                }
                if (!SessionContext.ValidateEmail(emailText.Text))
                {
                    email_err.Visibility = Visibility.Visible;
                }
            }
        }

        private bool UserIsExist(string password, string email)
        {
            User? currUser = worker.DbExecuteSQL<User>(
                "SELECT * FROM Users WHERE Email = @p0 AND Password = @p1",
                email,
                password
            ).FirstOrDefault();

            if (currUser != null)
            {
                SessionContext.CurrentUser = currUser;
                SessionContext.CurrentSettings = worker.DbExecuteSQL<Settings>(
                    "SELECT * FROM Settings WHERE UserId = @p0",
                    currUser.Id
                ).FirstOrDefault() ?? new Settings { ThemeId = 1, LangId = "en-US" };
                SessionContext.StartLangId = SessionContext.CurrentSettings.LangId;

                ApplicationLanguages.PrimaryLanguageOverride = SessionContext.StartLangId;
            }

            return currUser != null;
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            App.loginWindow.Content = App.GetService<SignUpPage>();
        }

        private void ForgotButton_Click(object sender, RoutedEventArgs e)
        {
            App.loginWindow.Content = App.GetService<ForgotPage>();
        }
    }
}
