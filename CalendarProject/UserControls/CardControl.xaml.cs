using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace CalendarProject.UserControls
{
    public sealed partial class CardControl : UserControl
    {
        public CardControl()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get => CardTextView.Text;
            set => CardTextView.Text = value;
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
            }
            _lastClickTime = now;
        }

    }
}
