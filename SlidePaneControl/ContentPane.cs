using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SlidePaneControl
{
    [ContentProperty(Name = "Content")]
    public sealed class ContentPane : Control
    {
        public ContentPane()
        {
            this.DefaultStyleKey = typeof(ContentPane);
        }

        #region ButtonVisibility

        public Visibility ButtonVisibility
        {
            get { return (Visibility)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.Register(
                "ButtonVisibility",
                typeof(Visibility),
                typeof(ContentPane),
                new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region IsAvailable

        public bool IsAvailable
        {
            get { return (bool)GetValue(IsAvailableProperty); }
            protected set { SetValue(IsAvailableProperty, value); }
        }

        public static readonly DependencyProperty IsAvailableProperty =
            DependencyProperty.Register(
                "IsAvailable",
                typeof(bool),
                typeof(ContentPane),
                new PropertyMetadata(false));

        #endregion

        #region Content

        public FrameworkElement Content
        {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set
            {
                SetValue(ContentProperty, value);
                this.IsAvailable = value != null;
            }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                "Content",
                typeof(FrameworkElement),
                typeof(ContentPane),
                new PropertyMetadata(null));

        #endregion

    }
}
