using CalendarProject.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace CalendarProject.Views;

public sealed partial class AddEventPage : Page
{
    public AddEventViewModel ViewModel
    {
        get;
    }

    public AddEventPage()
    {
        ViewModel = App.GetService<AddEventViewModel>();
        InitializeComponent();
    }
}
