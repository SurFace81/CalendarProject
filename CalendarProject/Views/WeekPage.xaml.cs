using CalendarProject.ViewModels;
using CalendarProject.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CalendarProject.UserControls;

namespace CalendarProject.Views
{
    public sealed partial class WeekPage : Page
    {
        public WeekViewModel ViewModel { get; }

        private DateTime dateMon;

        public WeekPage()
        {
            ViewModel = App.GetService<WeekViewModel>();
            dateMon = DateTime.Now;
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
                        dayCard.AddTask(item.Time.ToString(), TrimString(item.Header));
                    }
                }
            }

            addInfo.Text = GetWeekInterval();
        }

        private List<Event> GetDayEvents(DateTime day)
        {
            // TODO
            // Обращение к БД чтобы получить список задач на определенный день
            return new List<Event>();
        }

        private string TrimString(string str)
        {
            if (str.Length > 15)
            {
                return str.Substring(0, 10) + "...";
            }
            return str;
        }

        private string GetWeekInterval()
        {
            DateTime dateSun = dateMon.AddDays(6);

            return dateMon.Day.ToString() + "." + 
                   dateMon.Month.ToString() + "." +
                   dateMon.Year.ToString() + " - " +
                   dateSun.Day.ToString() + "." +
                   dateSun.Month.ToString() + "." +
                   dateSun.Year.ToString();
        }
    }
}