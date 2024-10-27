using CalendarProject.EntityFramework;
using CalendarProject.Models;
using CalendarProject.ViewModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CalendarProject.Views
{

    public sealed partial class AddEventPage : Page
    {
        private DbWorker dbWorker { get; }


        public AddEventViewModel ViewModel
        {
            get;
        }

        public AddEventPage()
        {
            this.InitializeComponent();
            dbWorker = App.GetService<DbWorker>();

        }

        private void NotificationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NotificationTimePanel.Visibility = Visibility.Visible;
        }

        private void NotificationCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NotificationTimePanel.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            string Header = EventTitleTextBox.Text;
            string Description = EventDescriptionTextBox.Text;
            DateTime Date = EventDatePicker.Date.Date + TimeSpan.Zero;
            DateTime Time = Date.Add(EventTimePicker.Time);

            int Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() switch
            {
                "Low" => 1,
                "Medium" => 2,
                "High" => 3,
                _ => 0
            };

            bool notificationsEnabled = NotificationCheckBox.IsChecked ?? false;
            DateTime NotifTime;

            if (notificationsEnabled)
            {
                var selectedItem = NotificationTimeComboBox.SelectedItem as ComboBoxItem;
                NotifTime = selectedItem != null
                    ? CalculateNotifTime(Time, selectedItem.Content.ToString())
                    : DateTime.Now;
            }
            else
            {
                NotifTime = DateTime.Now;
            }

            Event newEvent = new Event
            {
                UserId = SessionContext.CurrentUser.Id,
                Time = Time,
                Date = Date,
                NotifTime = NotifTime,
                Header = Header,
                Description = Description,
                Priority = Priority
            };

            dbWorker.DbAdd(newEvent);
            Frame.GoBack();

            Frame.Navigate(typeof(DayPage));


        }

        private DateTime CalculateNotifTime(DateTime eventDateTime, string NotifTimeStr)
        {
            int minutesBeforeEvent = NotifTimeStr switch
            {
                "5 minutes" => 5,
                "10 minutes" => 10,
                "15 minutes" => 10,
                "30 minutes" => 30,
                "1 hour" => 60,
                "2 hours" => 120,
                "1 day" => 3600,
                _ => 0
            };

            return eventDateTime.AddMinutes(-minutesBeforeEvent);
        }

    }
}