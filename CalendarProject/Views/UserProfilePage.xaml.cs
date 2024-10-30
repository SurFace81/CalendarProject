using CalendarProject.EntityFramework;
using CalendarProject.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Xml.Linq;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
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
<<<<<<< HEAD
        string name = UserNameTextBox.Text;
        string email = UserEmailTextBox.Text;
        string password = UserPasswordTextBox.Text;
        bool autoLogin = CheckBoxValue;
        int userId = SessionContext.CurrentUser.Id;
=======
        string Name = UserNameTextBox.Text;
        string Email = UserEmailTextBox.Text;
        string Password = UserPasswordTextBox.Password;
        bool AutoLogin = CheckBoxValue;
        
>>>>>>> aae4722b9f193404e40ecd4ab15033e0ba029122

        try
        {
            ValidateInput(name, email, password);
            UpdateUser(name, email, password, autoLogin, userId);
            ErrorMessage.Visibility = Visibility.Collapsed;
            
        }
        catch (ArgumentException ex)
        {
            // Отобразить сообщение об ошибке
            ErrorMessage.Text = ex.Message;
            ErrorMessage.Visibility = Visibility.Visible;
        }   
    }

    private void UpdateUser(string name, string email, string password, bool autoLogin, int userId)
    {

        var userToUpdate = dbWorker.GetUserById(userId);

        if (userToUpdate != null)
        {
            userToUpdate.Name = name;
            userToUpdate.Email = email;
            userToUpdate.Password = SessionContext.GetMD5Hash(password);
            userToUpdate.AutoLogin = autoLogin;

            dbWorker.DbUpdate(userToUpdate);
        }
    }

    private void ValidateInput(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Имя не может быть пустым.");
        }

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            throw new ArgumentException("Некорректный email.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Пароль не может быть пустым.");
        }
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        // Пустой обработчик для кнопки выхода из аккаунта
    }
}
