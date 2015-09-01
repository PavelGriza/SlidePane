using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SlidePaneControl
{
    [ContentProperty(Name = "Content")]
    public sealed partial class SlidePane : Control
    {
        #region Fields

        private Action _action;
        private const string LeftSlideButtonTemplateName = "LeftSlideButton";
        private const string RightSlideButtonTemplateName = "RightSlideButton";

        #endregion

        public SlidePane()
        {
            this.DefaultStyleKey = typeof(SlidePane);

            this.Loaded += (s, e) =>
            {
                InitGestureInteraction();
                InitButtonsBehavior();
            };
        }

        #region Content

        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                "Content",
                typeof(UIElement),
                typeof(SlidePane),
                new PropertyMetadata(null));

        #endregion


        #region LeftPane

        public ContentPane LeftPane
        {
            get { return (ContentPane)GetValue(LeftPaneProperty); }
            set { SetValue(LeftPaneProperty, value); }
        }

        public static readonly DependencyProperty LeftPaneProperty =
            DependencyProperty.Register(
                "LeftPane",
                typeof(ContentPane),
                typeof(SlidePane),
                new PropertyMetadata(
                    new ContentPane(),
                    OnLeftPanePropertyChanged));

        private static void OnLeftPanePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PanePropertyChanged(d, e, true);
        }

        #endregion

        #region LeftPaneButtonVisibility

        public Visibility LeftPaneButtonVisibility
        {
            get { return (Visibility)GetValue(LeftPaneButtonVisibilityProperty); }
            set { SetValue(LeftPaneButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LeftPaneButtonVisibilityProperty =
            DependencyProperty.Register(
                "LeftPaneButtonVisibility",
                typeof(Visibility),
                typeof(SlidePane),
                new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region IsLeftPaneOpen

        public bool IsLeftPaneOpen
        {
            get { return (bool)GetValue(IsLeftPaneOpenProperty); }
            set { SetValue(IsLeftPaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsLeftPaneOpenProperty =
            DependencyProperty.Register(
                "IsLeftPaneOpen",
                typeof(bool),
                typeof(SlidePane),
                new PropertyMetadata(false, OnIsLeftPaneOpenPropertyChanged));

        private static void OnIsLeftPaneOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IsPaneOpenPropertyChanged(d, e, true);
        }

        #endregion


        #region RightPane

        public ContentPane RightPane
        {
            get { return (ContentPane)GetValue(RightPaneProperty); }
            set { SetValue(RightPaneProperty, value); }
        }

        public static readonly DependencyProperty RightPaneProperty =
            DependencyProperty.Register(
                "RightPane",
                typeof(ContentPane),
                typeof(SlidePane),
                new PropertyMetadata(
                    new ContentPane(),
                    OnRightPanePropertyChanged));

        private static void OnRightPanePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PanePropertyChanged(d, e, false);
        }

        #endregion

        #region RightPaneButtonVisibility

        public Visibility RightPaneButtonVisibility
        {
            get
            {
                return this.RightPane == null
                    ? Visibility.Collapsed
                    : (Visibility)GetValue(RightPaneButtonVisibilityProperty);
            }
            set { SetValue(RightPaneButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RightPaneButtonVisibilityProperty =
            DependencyProperty.Register(
                "RightPaneButtonVisibility",
                typeof(Visibility),
                typeof(SlidePane),
                new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region IsRightPaneOpen

        public bool IsRightPaneOpen
        {
            get { return (bool)GetValue(IsRightPaneOpenProperty); }
            set { SetValue(IsRightPaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsRightPaneOpenProperty =
            DependencyProperty.Register(
                "IsRightPaneOpen",
                typeof(bool),
                typeof(SlidePane),
                new PropertyMetadata(false, OnIsRightPaneOpenPropertyChanged));

        private static void OnIsRightPaneOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IsPaneOpenPropertyChanged(d, e, false);
        }

        #endregion


        #region Private Methods

        private void InitButtonsBehavior()
        {
            var leftSlideButton = GetTemplateChild(LeftSlideButtonTemplateName) as Button;
            if (leftSlideButton != null)
                leftSlideButton.Click += (ss, ee) =>
                {
                    if (this.LeftPane.IsAvailable)
                    {
                        this.IsLeftPaneOpen = !this.IsLeftPaneOpen;
                    }
                };

            var rightSlideButton = GetTemplateChild(RightSlideButtonTemplateName) as Button;
            if (rightSlideButton != null)
                rightSlideButton.Click += (ss, ee) =>
                {
                    if (this.RightPane.IsAvailable)
                    {
                        this.IsRightPaneOpen = !this.IsRightPaneOpen;
                    }
                };
        }

        private static void IsPaneOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, bool isLeftPaneProperty)
        {
            var slidePane = d as SlidePane;
            if (slidePane == null)
            {
                throw new ArgumentException("Argument is not a SlidePane type", "d");
            }

            var isPaneOpen = (bool)e.NewValue;

            if (isLeftPaneProperty)
            {
                if (isPaneOpen)
                {
                    switch (slidePane._action)
                    {
                        case Action.Default:
                            slidePane._action = slidePane.IsRightPaneOpen ? Action.OpenLeftCloseRight : Action.OpenLeft;
                            break;
                    }
                    //slidePane.OpenLeftPane();
                }
                else
                {
                    switch (slidePane._action)
                    {
                        case Action.Default:
                            slidePane._action = Action.CloseLeft;
                            break;
                        case Action.OpenRightCloseLeft:
                            return;
                    }
                    //slidePane.CloseLeftPane();
                }
            }
            else
            {
                if (isPaneOpen)
                {
                    switch (slidePane._action)
                    {
                        case Action.Default:
                            slidePane._action = slidePane.IsLeftPaneOpen ? Action.OpenRightCloseLeft : Action.OpenRight;
                            break;
                    }
                    //slidePane.OpenRightPane();
                }
                else
                {
                    switch (slidePane._action)
                    {
                        case Action.Default:
                            slidePane._action = Action.CloseRight;
                            break;
                        case Action.OpenLeftCloseRight:
                            return;
                    }
                    //slidePane.CloseRightPane();
                }
            }

            // todo: call specifig method

            switch (slidePane._action)
            {
                case Action.OpenLeft:
                    slidePane.OpenLeftPane();
                    break;
                case Action.OpenRight:
                    slidePane.OpenRightPane();
                    break;
                case Action.CloseLeft:
                    slidePane.CloseLeftPane();
                    break;
                case Action.CloseRight:
                    slidePane.CloseRightPane();
                    break;
                case Action.OpenLeftCloseRight:
                    slidePane.OpenLeftCloseRightPane();
                    slidePane.IsRightPaneOpen = false;
                    break;
                case Action.OpenRightCloseLeft:
                    slidePane.OpenRightCloseLeftPane();
                    slidePane.IsLeftPaneOpen = false;
                    break;
            }

            slidePane._action = Action.Default;
        }

        private static void PanePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, bool isLeft)
        {
            var slidePane = d as SlidePane;
            if (slidePane == null)
            {
                throw new ArgumentException("Argument is not a SlidePane type", "d");
            }

            var pane = e.NewValue as ContentPane;

            if (pane == null) return;

            pane.Loaded += (s, args) =>
            {
                if (isLeft)
                {
                    slidePane.LeftSlidingWidth = pane.Content.ActualWidth;
                    slidePane.Container.Margin = new Thickness(
                        -slidePane.LeftSlidingWidth,
                        slidePane.Container.Margin.Top,
                        slidePane.Container.Margin.Right,
                        slidePane.Container.Margin.Bottom);
                }
                else
                {
                    slidePane.RightSlidingWidth = pane.Content.ActualWidth;
                    slidePane.Container.Margin = new Thickness(
                        slidePane.Container.Margin.Left,
                        slidePane.Container.Margin.Top,
                        -slidePane.RightSlidingWidth,
                        slidePane.Container.Margin.Bottom);
                }
            };
        }

        #endregion

    }
}
