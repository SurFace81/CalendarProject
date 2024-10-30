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
        string Name = UserNameTextBox.Text;
        string Email = UserEmailTextBox.Text;
        string Password = UserPasswordTextBox.Password;
        bool AutoLogin = CheckBoxValue;
        

        User newUser = new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            AutoLogin = AutoLogin,
        };

        dbWorker.DbAdd(newUser);
    }




    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        // Пустой обработчик для кнопки выхода из аккаунта
    }
}
