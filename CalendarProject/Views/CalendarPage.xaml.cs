using CalendarProject.UserControls;
using CalendarProject.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System.Globalization;
using CalendarProject.Helpers;

namespace CalendarProject.Views;

public sealed partial class CalendarPage : Page
{
    public CalendarViewModel ViewModel
    {
        get;
    }

    private readonly List<CardControl> cards = new();

    public CalendarPage()
    {
        ViewModel = App.GetService<CalendarViewModel>();
        InitializeComponent();

        InitCards();
        CalcMonth();
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e)
    {
        monthOffset -= 1;
        ResetCards();
        CalcMonth();
    }

    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
        monthOffset += 1;
        ResetCards();
        CalcMonth();
    }

    private void ResetCards()
    {
        foreach (var card in cards)
        {
            card.Visibility = Visibility.Visible;
        }
    }

    private int monthOffset = 0;
    private void CalcMonth()
    {
        DateTime now = DateTime.Now.AddMonths(monthOffset);
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
                dayNum += 1;
            }
            else
            {
                cards[i].Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
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
            // TODO: AddEvent
        }
    }
}
