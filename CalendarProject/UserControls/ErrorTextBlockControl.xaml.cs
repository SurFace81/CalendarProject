using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CalendarProject.UserControls
{
    public sealed partial class ErrorTextBlockControl : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(ErrorTextBlockControl),
                new PropertyMetadata(0.0, OnTextChanged));

        public static new readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(
            "Visibility",
            typeof(Visibility),
            typeof(ErrorTextBlockControl),
            new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

        public static new readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(
                "FontSize",
                typeof(double),
                typeof(ErrorTextBlockControl),
                new PropertyMetadata(14.0, OnFontSizeChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public new Visibility Visibility
        {
            get => (Visibility)GetValue(VisibilityProperty);
            set => SetValue(VisibilityProperty, value);
        }

        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public ErrorTextBlockControl()
        {
            this.InitializeComponent();
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ErrorTextBlockControl control)
            {
                control.infoText.Text = (string)e.NewValue;
            }
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ErrorTextBlockControl control)
            {
                control.errorTextBlockBack.Visibility = (Visibility)e.NewValue;
            }
        }

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ErrorTextBlockControl control)
            {
                control.infoText.FontSize = (double)e.NewValue;
            }
        }
    }
}
