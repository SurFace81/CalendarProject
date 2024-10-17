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
    public UserProfileViewModel ViewModel
    {
        get;
    }

    public UserProfilePage()
    {
        ViewModel = App.GetService<UserProfileViewModel>();
        InitializeComponent();
    }

    private void ChangeAvatarButton_Click(object sender, RoutedEventArgs e)
    {
        // Нужно сделать добавление картинки
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Пустой обработчик для кнопки сохранения информации
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        // Пустой обработчик для кнопки выхода из аккаунта
    }
}
