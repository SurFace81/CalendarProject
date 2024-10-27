using CalendarProject.Contracts.Services;
using CalendarProject.EntityFramework;
using CalendarProject.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CalendarProject
{
    public sealed partial class LoginWindow : WindowEx
    {
        private LaunchActivatedEventArgs args;
        private DbWorker worker;

        public LoginWindow(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();

            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
            Title = "AppDisplayName".GetLocalized();

            worker = App.GetService<DbWorker>();

            //Avatar.ImageSource = new BitmapImage(new Uri(Path.Combine(AppContext.BaseDirectory, "Assets/DefaultAvatar.jpg")));

            this.args = args;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePassword(passwText.Password) && ValidateEmail(emailText.Text))
            {
                if (UserIsExist(SessionContext.GetMD5Hash(passwText.Password), emailText.Text))
                {
                    await App.GetService<IActivationService>().ActivateAsync(args);
                    this.Close();
                }
                else
                {
                    userNotFoundMsg.Visibility = Visibility.Visible;
                }
            }
            else
            {
                passw_err.Visibility = Visibility.Collapsed;
                email_err.Visibility = Visibility.Collapsed;

                if (!ValidatePassword(passwText.Password))
                {
                    passw_err.Visibility = Visibility.Visible;
                }
                if (!ValidateEmail(emailText.Text))
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
                ).FirstOrDefault() ?? new Settings { ThemeId = 1, LangId = 1 };
            }

            return currUser != null;
        }

        private bool ValidatePassword(string password)
        {
            return !string.IsNullOrEmpty(password);
        }

        private bool ValidateEmail(string email)
        {
            Regex emailRegex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return !string.IsNullOrWhiteSpace(email) && emailRegex.IsMatch(email);
        }
    }
}
