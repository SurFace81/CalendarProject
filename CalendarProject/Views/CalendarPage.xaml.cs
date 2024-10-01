using CalendarProject.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace CalendarProject.Views;

public sealed partial class CalendarPage : Page
{
    public CalendarViewModel ViewModel
    {
        get;
    }

    public CalendarPage()
    {
        ViewModel = App.GetService<CalendarViewModel>();
        InitializeComponent();
    }
}
