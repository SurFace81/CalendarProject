using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System.Globalization;

using CalendarProject.UserControls;
using CalendarProject.ViewModels;
using CalendarProject.Contracts.Services;
using CalendarProject.Models;
using Windows.UI;
using CalendarProject.EntityFramework;

namespace CalendarProject.Views;

public sealed partial class CalendarPage : Page
{
    public CalendarViewModel ViewModel { get; }

    private readonly List<CardControl> cards = new();
    private DbWorker dbWorker;

    public CalendarPage()
    {
        ViewModel = App.GetService<CalendarViewModel>();
        dbWorker = App.GetService<DbWorker>();
        InitializeComponent();

        InitCards();
        CalcMonth();
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e)
    {
        monthOffset -= 1;
        CalcMonth();
    }

    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
        monthOffset += 1;
        CalcMonth();
    }

    private void ResetCards()
    {
        foreach (var card in cards)
        {
            card.Visibility = Visibility.Visible;

            (card.FindName("cardBorder") as Border)!.BorderThickness = new Thickness(1);
            (card.FindName("cardBorder") as Border)!.BorderBrush = (SolidColorBrush)Application.Current.Resources["ControlStrokeColorDefaultBrush"];
        }
    }

    private int monthOffset = 0;
    private DateTime now;
    private void CalcMonth()
    {
        ResetCards();

        now = DateTime.Now.AddMonths(monthOffset);
        //addInfo_text.Text = now.ToString("MMMM", new CultureInfo("ru-RU")) + " " + now.Year;
        addInfo_text.Text = now.ToString("MMMM", new CultureInfo("en-US")) + " " + now.Year;

        int firstDayOfMonth = (int)(new DateTime(now.Year, now.Month, 1).DayOfWeek);
        firstDayOfMonth = (firstDayOfMonth == (int)DayOfWeek.Sunday) ? 7 : firstDayOfMonth;
        for (var i = 0; i < firstDayOfMonth - 1; i++)
        {
            cards[i].Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
        var dayNum = 1;
        for (var i = 0; i < cards.Count(); i++)
        {
            if (cards[i].Visibility == Microsoft.UI.Xaml.Visibility.Visible && dayNum <= daysInMonth)
            {
                cards[i].Text = dayNum.ToString();
                if (dayNum == DateTime.Now.Day && (now.Month == DateTime.Now.Month && now.Year == DateTime.Now.Year))
                {
                    (cards[i].FindName("cardBorder") as Border)!.BorderThickness = new Thickness(3);
                    (cards[i].FindName("cardBorder") as Border)!.BorderBrush = new SolidColorBrush((Color)Application.Current.Resources["SystemColorControlAccentColor"]);
                }

                GetPriorities(cards[i], dayNum);
                dayNum += 1;
            }
            else
            {
                cards[i].Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }
    }

    private void GetPriorities(CardControl card, int day)
    {
        card.ClearPriorities();

        var eventsPriorities = dbWorker.DbExecuteSQL<Event>(
            "SELECT * FROM Events WHERE Date = @p0 AND UserId = @p1",
            new DateTime(now.Year, now.Month, day).ToString("yyyy-MM-dd HH:mm:ss"),
            SessionContext.CurrentUser.Id
        );

        foreach (var item in eventsPriorities)
        {
            switch (item.Priority)
            {
                case 1:
                    card.LowPriority = true;
                    break;
                case 2:
                    card.MediumPriority = true;
                    break;
                case 3:
                    card.HighPriority = true;
                    break;
                default: break;
            }
        }
    }

    private void InitCards()
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(ContentArea); i++)
        {
            var child = VisualTreeHelper.GetChild(ContentArea, i);
            if (child is CardControl)
            {
                cards.Add((CardControl)child);
            }
        }

        for (var i = 0; i < cards.Count(); i++)
        {
            cards[i].OnClicked += CalendarPageCard_OnClicked;
        }
    }

    private void CalendarPageCard_OnClicked(object? sender, EventArgs e)
    {
        App.GetService<INavigationService>().NavigateTo(
            typeof(DayViewModel).FullName!,
            parameter: new EventStartupData { Date = new DateTime(now.Year, now.Month, Convert.ToInt32((sender as CardControl)?.Text)) }
        );
    }
}
