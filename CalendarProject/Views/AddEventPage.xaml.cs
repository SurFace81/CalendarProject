using CalendarProject.ViewModels;
using Microsoft.UI.Xaml;
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
        this.InitializeComponent();
    }

    // Метод для обработки включения уведомлений
    private void NotificationCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        NotificationTimePanel.Visibility = Visibility.Visible;
    }

    // Метод для обработки выключения уведомлений
    private void NotificationCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        NotificationTimePanel.Visibility = Visibility.Collapsed;
    }

    // Метод для сохранения события
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Получение значений полей
        string eventDescription = EventDescriptionTextBox.Text;
        DateTime eventDate = EventDatePicker.Date.DateTime;
        TimeSpan eventTime = EventTimePicker.Time;
        string priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        bool notificationsEnabled = NotificationCheckBox.IsChecked ?? false;

        string notificationTime = null;
        if (notificationsEnabled)
        {
            notificationTime = (NotificationTimeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        // Логика для сохранения события
        // Например, можно сохранить данные в базе данных или отправить их в другой сервис

        // Переход на предыдущую страницу или показ уведомления о сохранении
    }

}
