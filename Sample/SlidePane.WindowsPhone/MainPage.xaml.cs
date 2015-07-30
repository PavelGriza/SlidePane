using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SlidePane
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
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
