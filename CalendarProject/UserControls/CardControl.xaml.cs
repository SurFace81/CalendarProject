using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SQLitePCL;

namespace CalendarProject.UserControls
{
    public sealed partial class CardControl : UserControl
    {
        public event EventHandler OnClicked;

        public CardControl()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get => CardTextView.Text;
            set => CardTextView.Text = value;
        }

        public bool HighPriority
        {
            get => highPrty.Visibility == Microsoft.UI.Xaml.Visibility.Visible;
            set => highPrty.Visibility = value ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public bool MediumPriority
        {
            get => mediumPrty.Visibility == Microsoft.UI.Xaml.Visibility.Visible;
            set => mediumPrty.Visibility = value ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public bool LowPriority
        {
            get => lowPrty.Visibility == Microsoft.UI.Xaml.Visibility.Visible;
            set => lowPrty.Visibility = value ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public void ClearPriorities()
        {
            LowPriority = false;
            MediumPriority = false;
            HighPriority = false;
        }

        private DateTime _lastClickTime;
        private const int DoubleClickTime = 500;
        private void Card_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            if ((now - _lastClickTime).TotalMilliseconds <= DoubleClickTime)
            {
                HighlightAnimation.Stop();
                HighlightAnimation.Begin();

                OnClicked?.Invoke(this, EventArgs.Empty);
            }
            _lastClickTime = now;
        }
    }
}
