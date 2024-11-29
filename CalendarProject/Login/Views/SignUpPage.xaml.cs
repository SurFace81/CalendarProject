using CalendarProject.EntityFramework;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace CalendarProject.Views
{
    public sealed partial class SignUpPage : Page
    {
        private DbWorker worker;

        public SignUpPage()
        {
            this.InitializeComponent();

            worker = App.GetService<DbWorker>();
        }

        private void GoPrevPage()
        {
            App.loginWindow.Content = App.GetService<LoginPage>();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoPrevPage();
        }

        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            if (IsUserExist(emailTb.Text))
            {
                ShowDialog();
                return;
            }

            worker.DbAdd<User>(new User
            {
                Name = nameTb.Text,
                Email = emailTb.Text,
                Password = SessionContext.GetMD5Hash(passwTb.Password)
            });

            GoPrevPage();
        }

        private async void ShowDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = "A user with this email already exists.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            _ = await dialog.ShowAsync();
        }

        private bool IsUserExist(string email)
        {
            return (worker.DbExecuteSQL<User>("SELECT * FROM Users WHERE Email = @p0", email).Count > 0);
        }

        private bool Validate()
        {
            name_err.Visibility = Visibility.Collapsed;
            passw_err.Visibility = Visibility.Collapsed;
            email_err.Visibility = Visibility.Collapsed;

            bool flag = true;
            if (string.IsNullOrEmpty(nameTb.Text))
            {
                name_err.Visibility = Visibility.Visible;
                flag = false;
            }
            if (!SessionContext.ValidateEmail(emailTb.Text))
            {
                email_err.Visibility = Visibility.Visible;
                flag = false;
            }
            if (string.IsNullOrEmpty(passwTb.Password))
            {
                passw_err.Visibility = Visibility.Visible;
                flag = false;
            }
            return flag;
        }
    }
}
