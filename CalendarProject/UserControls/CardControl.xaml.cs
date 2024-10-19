using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

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
