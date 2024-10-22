using CalendarProject.ViewModels;
using CalendarProject.EntityFramework;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CalendarProject.UserControls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Windows.UI;

namespace CalendarProject.Views
{
    public sealed partial class WeekPage : Page
    {
        public WeekViewModel ViewModel { get; }
        private DbWorker dbWorker { get; }

        private DateTime dateMon;

        public WeekPage()
        {
            ViewModel = App.GetService<WeekViewModel>();
            dbWorker = App.GetService<DbWorker>();

            DateTime today = DateTime.Today;
            DayOfWeek currDay = today.DayOfWeek;
            int daysToMonday = (int)currDay - (int)DayOfWeek.Monday;
            dateMon = DateTime.Today.AddDays(-daysToMonday);

            InitializeComponent();
            WeekUpdate();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            dateMon = dateMon.AddDays(-7);
            WeekUpdate();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            dateMon = dateMon.AddDays(7);
            WeekUpdate();
        }

        private void WeekUpdate()
        {
            for (var i = 0; i < 7; i++)
            {
                List<Event> events = GetDayEvents(dateMon.AddDays(i));

                if (week_SP.Children[i] is DayCardControl dayCard)
                {
                    dayCard.ClearTasks();
                    foreach (var item in events)
                    {
                        dayCard.AddTask(item.Time.ToString("d"), TrimString(item.Header), GetPriorityColor(item.Priority));
                    }
                }
            }

            addInfo.Text = GetWeekInterval();
        }

        private List<Event> GetDayEvents(DateTime day)
        {
            List<Event> events = dbWorker.DbExecuteSQL<Event>(
                "SELECT * FROM Events WHERE UserId = {0} AND Date = {1}",
                SessionContext.CurrentUser.Id, 
                day.ToString("yyyy-MM-dd HH:mm:ss")
            );

            return events;
        }

        private string TrimString(string str)
        {
            if (str.Length > 18)
            {
                return str.Substring(0, 18) + "...";
            }
            return str;
        }

        private string GetWeekInterval()
        {
            DateTime dateSun = dateMon.AddDays(6);

            return dateMon.Day.ToString().PadLeft(2, '0') + "." + 
                   dateMon.Month.ToString().PadLeft(2, '0') + "." +
                   dateMon.Year.ToString().Substring(2, 2) + " - " +
                   dateSun.Day.ToString().PadLeft(2, '0') + "." +
                   dateSun.Month.ToString().PadLeft(2, '0') + "." +
                   dateSun.Year.ToString().Substring(2, 2);
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
}