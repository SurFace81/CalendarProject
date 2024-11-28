using CalendarProject.ViewModels;
using CalendarProject.EntityFramework;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;
using System.Collections.ObjectModel;
using CalendarProject.Contracts.Services;
using CalendarProject.Models;
using Microsoft.UI.Xaml.Navigation;

namespace CalendarProject.Views
{
    public sealed partial class DayPage : Page
    {
        public DayViewModel ViewModel { get; }
        private DbWorker dbWorker { get; }

        private DateTime dateNow;

        public ObservableCollection<EventsListItem> DayEvents { get; set; } = new();

        public DayPage()
        {
            ViewModel = App.GetService<DayViewModel>();
            dbWorker = App.GetService<DbWorker>();

            dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            InitializeComponent();
            datePicker.SelectedDate = dateNow;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is EventDto esd)
                dateNow = esd.Date;

            datePicker.SelectedDate = dateNow;
            UpdateDayCard();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            dateNow = dateNow.AddDays(-1);
            datePicker.SelectedDate = dateNow;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            dateNow = dateNow.AddDays(1);
            datePicker.SelectedDate = dateNow;
        }

        private void UpdateDayCard()
        {
            DayEvents.Clear();
            foreach (var item in GetDayEvents(dateNow))
            {
                DayEvents.Add(new EventsListItem
                {
                    Id = item.Id,
                    Time = item.Time.ToString("HH:mm:ss"),
                    Description = TrimString(item.Description, 80),
                    PriorityColor = GetPriorityColor(item.Priority),
                });
            }
            noEventsMessage.Visibility = (DayEvents == null || DayEvents.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private List<Event> GetDayEvents(DateTime date)
        {
            return dbWorker.DbExecuteSQL<Event>(
                "SELECT * FROM Events WHERE UserId = @p0 AND Date = @p1",
                SessionContext.CurrentUser.Id,
                date.ToString("yyyy-MM-dd HH:mm:ss")
            );
        }

        private void OnAddEventButtonClick(object sender, RoutedEventArgs e)
        {
            App.GetService<INavigationService>().NavigateTo(
                typeof(AddEventViewModel).FullName!,
                parameter: new EventDto { Date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day) }
            );
        }

        private void OnEditItemClick(object sender, RoutedEventArgs e)
        {
            // TODO
            //var menuFlyoutItem = sender as MenuFlyoutItem
            //var item = menuFlyoutItem?.DataContext as EventsListItem;

            //if (item != null)
            //{
            //    App.GetService<INavigationService>().NavigateTo(
            //        typeof(AddEventViewModel).FullName!,
            //        parameter: new EventEditDto { eventForEdit = new Event { } }
            //    );
            //}
        }

        private void OnDeleteItemClick(object sender, RoutedEventArgs e)
        {
            var listViewItem = (sender as MenuFlyoutItem)?.DataContext as EventsListItem;

            if (listViewItem != null)
            {
                dbWorker.DbDelete<Event>(listViewItem.Id);
                DayEvents.Remove(listViewItem);
            }
        }

        private void OnSelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs e)
        {
            if (e.NewDate.HasValue)
            {
                dateNow = e.NewDate.Value.DateTime;
                UpdateDayCard();
            }
        }

        private string TrimString(string str, int size = 18)
        {
            return str.Length > size ? str.Substring(0, size) + "..." : str;
        }

        private SolidColorBrush GetPriorityColor(int priority)
        {
            switch (priority)
            {
                case 1:
                    return new SolidColorBrush(Colors.Green);
                case 2:
                    return new SolidColorBrush(Colors.Gold);
                case 3:
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush((Color)Application.Current.Resources["ControlFillColorDefault"]);
            }
        }
    }

    public class EventsListItem
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public SolidColorBrush PriorityColor { get; set; }
    }
}
