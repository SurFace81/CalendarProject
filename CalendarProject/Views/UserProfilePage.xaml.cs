using CalendarProject.EntityFramework;
using CalendarProject.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace CalendarProject.Views;


public sealed partial class UserProfilePage : Page
{
    private DbWorker dbWorker { get; }
    public bool CheckBoxValue { get; private set; }

    public UserProfileViewModel ViewModel
    {
        get;
    }

    public UserProfilePage()
    {
        ViewModel = App.GetService<UserProfileViewModel>();
        dbWorker = App.GetService<DbWorker>();
        InitializeComponent();
    }

    private void ChangeAvatarButton_Click(object sender, RoutedEventArgs e)
    {
        // Нужно сделать добавление картинки
    }

    private void AutoLoginCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        bool CheckBoxValue = true;
    }

    private void AutoLoginCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        bool CheckBoxValue = false;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string name = UserNameTextBox.Text;
        string email = UserEmailTextBox.Text;
        string password = UserPasswordTextBox.Password;
        bool autoLogin = CheckBoxValue;
        int userId = SessionContext.CurrentUser.Id;

        if (ValidateInput(name, email, password))
        {
            User? userToUpdate = dbWorker.DbExecuteSQL<User>(
                "SELECT * FROM Users WHERE Id = @p0",
                userId
            ).FirstOrDefault();

            if (userToUpdate != null)
            {
                userToUpdate.Name = name;
                userToUpdate.Email = email;
                userToUpdate.Password = SessionContext.GetMD5Hash(password);
                userToUpdate.AutoLogin = autoLogin;

                dbWorker.DbUpdate(userToUpdate);
            }
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
