using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CalendarProject.Models;
using Microsoft.UI.Xaml.Input;

namespace CalendarProject.UserControls
{
    public sealed partial class DayCardControl : UserControl
    {
        public string DayName
        {
            get; set;
        }

        public DayCardControl()
        {
            this.InitializeComponent();

            AddTask("08:00", "Morning meeting");
            AddTask("12:00", "Lunch break");
            AddTask("03:00", "Project discussion");
        }

        private void Card_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private void AddTask(string time, string description)
        {
            TasksListView.Items.Add(new TaskControl
            {
                TaskTime = time,
                TaskDescription = description
            });
        }
    }
}
