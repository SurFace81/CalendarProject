using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace CalendarProject.UserControls
{
    public sealed partial class PriorityDisplayControl : UserControl
    {
        public PriorityDisplayControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(
                "Size",
                typeof(double),
                typeof(PriorityDisplayControl),
                new PropertyMetadata(0.0, OnSizeChanged));

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(
                "BorderColor",
                typeof(Brush),
                typeof(PriorityDisplayControl),
                new PropertyMetadata(0.0, OnColorChanged));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PriorityDisplayControl control)
            {
                control.PriorityBorder.Width = control.Size;
                control.PriorityBorder.Height = control.Size;
                control.PriorityBorder.CornerRadius = new CornerRadius(control.Size / 2);
            }
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PriorityDisplayControl control)
            {
                control.PriorityBorder.Background = (Brush)e.NewValue;
            }            
        }
    }
}