using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SlidePane
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ButtonLeft_OnClick(object sender, RoutedEventArgs e)
        {
            this.SlidePane.IsLeftPaneOpen = !this.SlidePane.IsLeftPaneOpen;
        }

        private void ButtonRight_OnClick(object sender, RoutedEventArgs e)
        {
            this.SlidePane.IsRightPaneOpen = !this.SlidePane.IsRightPaneOpen;
        }
    }
}
