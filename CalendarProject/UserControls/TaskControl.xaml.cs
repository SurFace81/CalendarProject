using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CalendarProject.UserControls
{
    public sealed partial class TaskControl : UserControl
    {
        public string TaskTime
        {
            get
            {
                return (string)GetValue(TaskTimeProperty);
            }
            set
            {
                SetValue(TaskTimeProperty, value);
            }
        }

        public static readonly DependencyProperty TaskTimeProperty =
            DependencyProperty.Register("TaskTime", typeof(string), typeof(TaskControl), new PropertyMetadata(string.Empty));

        public string TaskDescription
        {
            get
            {
                return (string)GetValue(TaskDescriptionProperty);
            }
            set
            {
                SetValue(TaskDescriptionProperty, value);
            }
        }

        public static readonly DependencyProperty TaskDescriptionProperty =
            DependencyProperty.Register("TaskDescription", typeof(string), typeof(TaskControl), new PropertyMetadata(string.Empty));

        public TaskControl()
        {
            this.InitializeComponent();
        }
    }
}
