using CalendarProject.Helpers;
using Microsoft.UI.Xaml;

namespace CalendarProject
{
    public sealed partial class LoginWindow : WindowEx
    {
        public static LaunchActivatedEventArgs args { get; private set; }    

        public LoginWindow(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();

            AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
            Title = "AppDisplayName".GetLocalized();

            LoginWindow.args = args;
        }
    }
}
