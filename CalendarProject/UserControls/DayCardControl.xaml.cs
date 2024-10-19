using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CalendarProject.Models;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace CalendarProject.UserControls
{
    public sealed partial class DayCardControl : UserControl
    {
        public string DayName { get; set; }

        public DayCardControl()
        {
            this.InitializeComponent();
        }

        private void Card_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        public void ClearTasks()
        {
            TasksListView.Items.Clear();
        }

        public void AddTask(string time, string description, SolidColorBrush brush)
        {
            TasksListView.Items.Add(new TaskControl
            {
                TaskTime = time,
                TaskDescription = description,
                TaskPriorityColor = brush
            });
        }
    }
}
