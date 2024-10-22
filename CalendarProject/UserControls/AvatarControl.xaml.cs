using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace CalendarProject.UserControls
{
    public sealed partial class AvatarControl : UserControl
    {
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(
                "Size",
                typeof(double),
                typeof(AvatarControl),
                new PropertyMetadata(0.0, OnContentChanged));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(
                "ImageSource",
                typeof(BitmapImage),
                typeof(AvatarControl),
                new PropertyMetadata(null, OnContentChanged));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public BitmapImage ImageSource
        {
            get => (BitmapImage)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public AvatarControl()
        {
            this.InitializeComponent();
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AvatarControl control)
            {
                control.AvatarBack.Width = control.Size;
                control.AvatarBack.Height = control.Size;
                control.AvatarBack.CornerRadius = new CornerRadius(control.Size / 2);

                if (control.ImageSource != null)
                {
                    control.AvatarContent.Content = new Microsoft.UI.Xaml.Controls.Image
                    {
                        Source = control.ImageSource,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
                else
                {
                    control.AvatarContent.Content = new TextBlock
                    {
                        FontFamily = new Microsoft.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets"),
                        Text = "\uE77B",
                        FontSize = control.Size * 0.45
                    };
                }
            }
        }
    }
}