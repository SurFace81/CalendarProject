using CalendarProject.ViewModels;

using Microsoft.UI.Xaml.Controls;

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
}
