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
            if (Validate()) return;

            worker.DbAdd<User>(new User
            {
                Name = nameTb.Text,
                Email = emailTb.Text,
                Password = SessionContext.GetMD5Hash(passwTb.Password),
                AutoLogin = (bool)autoLoginCb.IsChecked!
            });

            GoPrevPage();
        }

        private bool Validate()
        {
            name_err.Visibility = Visibility.Collapsed;
            passw_err.Visibility = Visibility.Collapsed;
            email_err.Visibility = Visibility.Collapsed;

            bool flag = false;
            if (string.IsNullOrEmpty(nameTb.Text))
            {
                name_err.Visibility = Visibility.Visible;
                flag = true;
            }
            if (!SessionContext.ValidateEmail(emailTb.Text))
            {
                email_err.Visibility = Visibility.Visible;
                flag = true;
            }
            if (string.IsNullOrEmpty(passwTb.Password))
            {
                passw_err.Visibility = Visibility.Visible;
                flag = true;
            }
            return flag;
        }
    }
}
