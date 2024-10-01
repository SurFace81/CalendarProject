using CalendarProject.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace CalendarProject.Views;

public sealed partial class WeekPage : Page
{
    public WeekViewModel ViewModel
    {
        get;
    }

    public WeekPage()
    {
        ViewModel = App.GetService<WeekViewModel>();
        InitializeComponent();
    }
}
