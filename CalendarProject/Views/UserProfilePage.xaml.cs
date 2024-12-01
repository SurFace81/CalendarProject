using CalendarProject.EntityFramework;
using CalendarProject.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace CalendarProject.Views
{
    public sealed partial class UserProfilePage : Page
    {
        private DbWorker dbWorker { get; }

        public UserProfileViewModel ViewModel { get; }

        private static string logoPath;

        public UserProfilePage()
        {
            ViewModel = App.GetService<UserProfileViewModel>();
            dbWorker = App.GetService<DbWorker>();
            InitializeComponent();

            if (logoPath != null)
            {
                UserAvatar.ImageSource = new BitmapImage(new Uri(logoPath));
            }
            UserNameTextBox.Text = SessionContext.CurrentUser.Name;
            UserEmailTextBox.Text = SessionContext.CurrentUser.Email;
        }

        private async void ChangeAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fop = new()
            {
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { ".jpg", ".png" }
            };

            nint windowHandle = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(fop, windowHandle);

            StorageFile file = await fop.PickSingleFileAsync();
            if (file != null)
            {
                logoPath = file.Path;
                UserAvatar.ImageSource = new BitmapImage(new Uri(logoPath));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = UserNameTextBox.Text;
            string email = UserEmailTextBox.Text;
            string password = UserPasswordTextBox.Password;
            var user = SessionContext.CurrentUser;

            if (ValidateInput(name, email, password))
            {
                user.Name = name;
                user.Email = email;
                user.Password = SessionContext.GetMD5Hash(password);
                user.Logo = logoPath;

                dbWorker.DbUpdate(user);
            }
        }

        private bool ValidateInput(string name, string email, string password)
        {
            bool flag = false;

            name_err.Visibility = string.IsNullOrWhiteSpace(name) ? Visibility.Visible : Visibility.Collapsed;
            email_err.Visibility = string.IsNullOrWhiteSpace(email) || !SessionContext.ValidateEmail(email) ? Visibility.Visible : Visibility.Collapsed;
            passw_err.Visibility = string.IsNullOrWhiteSpace(password) ? Visibility.Visible : Visibility.Collapsed;

            flag = name_err.Visibility == Visibility.Visible || email_err.Visibility == Visibility.Visible || passw_err.Visibility == Visibility.Visible;

            return !flag;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.loginWindow = new LoginWindow(App.LaunchArgs);
            App.loginWindow.Content = App.GetService<LoginPage>();
            App.loginWindow.Activate();
            App.MainWindow.Hide();
        }
    }
}