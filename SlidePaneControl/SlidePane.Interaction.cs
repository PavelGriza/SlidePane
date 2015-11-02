using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using SlidePaneControl.Extensions;

namespace SlidePaneControl
{
    public sealed partial class SlidePane
    {
        #region Fields

        public const string RootGridTemplateName = "RootGrid";
        private double _leftDragDistance; // the min distance of translation for finishing open/close process
        private double _rightDragDistance; // the min distance of translation for finishing open/close process
        private double _leftSlidingWidth; // max width of sliding
        private double _rightSlidingWidth; // max width of sliding
        private bool _isLeftPaneActualOpen; // actual left pane opening state
        private bool _isRightPaneActualOpen; // actual right pane opening state
        private double _currentX;

        #endregion

        public FrameworkElement Container { get; set; }

        public double LeftSlidingWidth
        {
            get { return _leftSlidingWidth; }
            set
            {
                _leftSlidingWidth = value;
                _leftDragDistance = _leftSlidingWidth / 3;
            }
        }

        public double RightSlidingWidth
        {
            get { return _rightSlidingWidth; }
            set
            {
                _rightSlidingWidth = value;
                _rightDragDistance = _rightSlidingWidth / 3;
            }
        }

        #region Private Methodes

        private void InitGestureInteraction()
        {
            this.Container = (FrameworkElement)GetTemplateChild(RootGridTemplateName);
            this.Container.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateRailsX;

            this.Container.ManipulationStarted += (s, e) =>
            {
                var elements = GetCapturedElements(s as UIElement);
                foreach (var element in elements)
                {
                    element.ReleasePointerCaptures();
                }
            };

            this.Container.ManipulationDelta += (s, e) =>
            {
                var x = e.Cumulative.Translation.X;
                PerformSliding(x);
            };

            this.Container.ManipulationCompleted += (s, e) =>
            {
                var x = e.Cumulative.Translation.X;
                FinalizeSliding(x);
            };
        }

        private static List<UIElement> GetCapturedElements(UIElement element)
        {
            List<UIElement> elements = new List<UIElement>();

            if (element.PointerCaptures != null)
            {
                elements.Add(element);
            }

            if (element is ContentPresenter)
            {
                var content = ((ContentPresenter) element).Content as UIElement;

                if (content != null)
                {
                    elements.AddRange(GetCapturedElements(content));
                }
            }

            if (element is ContentControl)
            {
                var content = ((ContentControl)element).Content as UIElement;

                if (content != null)
                {
                    elements.AddRange(GetCapturedElements(content));
                }
            }

            if (element is Panel)
            {
                foreach (UIElement child in ((Panel)element).Children)
                {
                    elements.AddRange(GetCapturedElements(child));
                }
            }

            if (element is Border)
            {
                elements.AddRange(GetCapturedElements(((Border)element).Child));
            }

            if (element is ItemsControl)
            {
                foreach (UIElement item in ((ItemsControl)element).Items.Where(i => i is UIElement))
                {
                    elements.AddRange(GetCapturedElements(item));
                }
            }

            return elements;
        }

        private void PerformSliding(double x)
        {
            double delta;
            TranslateTransform transform;

            if (x > 0)
            {
                if (this.RightPane.IsAvailable && this.IsRightPaneOpen) // close right pane
                {
                    delta = x <= this.RightSlidingWidth
                        ? x - this.RightSlidingWidth
                        : 0;
                    transform = new TranslateTransform() { X = delta };
                }
                else if (this.LeftPane.IsAvailable && !this.IsLeftPaneOpen) // open left pane
                {
                    delta = x <= this.LeftSlidingWidth
                        ? x
                        : this.LeftSlidingWidth;
                    transform = new TranslateTransform { X = delta };
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (this.LeftPane.IsAvailable && this.IsLeftPaneOpen) // close left pane
                {
                    delta = x >= -this.LeftSlidingWidth
                        ? -x
                        : this.LeftSlidingWidth;
                    transform = new TranslateTransform { X = this.LeftSlidingWidth - delta };
                }
                else if (this.RightPane.IsAvailable && !this.IsRightPaneOpen) // open right pane
                {
                    delta = x >= -this.RightSlidingWidth
                        ? x
                        : -this.RightSlidingWidth;
                    transform = new TranslateTransform { X = delta };
                }
                else
                {
                    return;
                }
            }

            this.Container.RenderTransform = transform;
        }

        private void FinalizeSliding(double x)
        {
            _currentX = x;

            if (x > 0)
            {
                if (this.RightPane.IsAvailable)
                {
                    if (this.IsRightPaneOpen)
                    {
                        if (x < _rightDragDistance)
                        {
                            OpenRightPane(); // prevent tab closing
                        }
                        else
                        {
                            this.IsRightPaneOpen = false;
                        }

                        return;
                    }
                }

                if (this.LeftPane.IsAvailable)
                {
                    if (!this.IsLeftPaneOpen)
                    {
                        if (x < _leftDragDistance)
                        {
                            CloseLeftPane(); // prevent tab opening
                        }
                        else
                        {
                            this.IsLeftPaneOpen = true;
                        }

                        return;
                    }
                }
            }
            else
            {
                if (this.LeftPane.IsAvailable)
                {
                    if (this.IsLeftPaneOpen)
                    {
                        if (Math.Abs(x) < _leftDragDistance)
                        {
                            OpenLeftPane(); // prevent tab closing
                        }
                        else
                        {
                            this.IsLeftPaneOpen = false;
                        }

                        return;
                    }
                }

                if (this.RightPane.IsAvailable)
                {
                    if (!this.IsRightPaneOpen)
                    {
                        if (Math.Abs(x) < _rightDragDistance)
                        {
                            CloseRightPane();
                        }
                        else
                        {
                            this.IsRightPaneOpen = true;
                        }

                        return;
                    }
                }
            }
        }

        private void OpenLeftPane()
        {
            var currentX = _currentX;
            double startPosition;

            if (_isLeftPaneActualOpen)
            {
                startPosition = this.LeftSlidingWidth + currentX; // currentX < 0
            }
            else
            {
                startPosition = currentX <= this.LeftSlidingWidth ? currentX : this.LeftSlidingWidth;
            }

            this.Container.Animate(from: startPosition, to: this.LeftSlidingWidth, fullDistance: this.LeftSlidingWidth);
            _isLeftPaneActualOpen = true;
            _currentX = 0;
        }

        private void CloseLeftPane()
        {
            var currentX = _currentX;
            currentX = Math.Abs(currentX);
            double startPosition;

            if (_isLeftPaneActualOpen)
            {
                startPosition = currentX <= this.LeftSlidingWidth ? this.LeftSlidingWidth - currentX : 0; // currentX < 0
            }
            else
            {
                startPosition = currentX;
            }

            this.Container.Animate(from: startPosition, to: 0, fullDistance: this.LeftSlidingWidth);
            _isLeftPaneActualOpen = false;
            _currentX = 0;
            _action = Action.Default;
        }

        private void OpenRightPane()
        {
            var currentX = _currentX;
            double startPosition;

            if (_isRightPaneActualOpen)
            {
                startPosition = currentX - this.RightSlidingWidth; // currentX < 0
            }
            else
            {
                startPosition = Math.Abs(currentX) <= this.RightSlidingWidth ? currentX : -this.RightSlidingWidth;
            }

            this.Container.Animate(from: startPosition, to: -this.RightSlidingWidth, fullDistance: this.RightSlidingWidth);
            _isRightPaneActualOpen = true;
            _currentX = 0;
        }

        private void CloseRightPane()
        {
            var currentX = _currentX;
            currentX = Math.Abs(currentX);
            double startPosition;

            if (_isRightPaneActualOpen)
            {
                startPosition = currentX <= this.RightSlidingWidth ? currentX - this.RightSlidingWidth : 0; // currentX < 0
            }
            else
            {
                startPosition = -currentX;
            }

            this.Container.Animate(from: startPosition, to: 0, fullDistance: this.RightSlidingWidth);
            _isRightPaneActualOpen = false;
            _currentX = 0;
        }

        private void OpenLeftCloseRightPane()
        {
            var currentX = _currentX;
            double startPosition = -(this.RightSlidingWidth + currentX);
            this.Container.Animate(from: startPosition, to: this.LeftSlidingWidth, fullDistance: this.LeftSlidingWidth + this.RightSlidingWidth);
            _isLeftPaneActualOpen = true;
            _isRightPaneActualOpen = false;
            _currentX = 0;
        }

        private void OpenRightCloseLeftPane()
        {
            var currentX = _currentX;
            var startPosition = this.LeftSlidingWidth + currentX;
            this.Container.Animate(from: startPosition, to: -this.RightSlidingWidth, fullDistance: this.RightSlidingWidth + this.LeftSlidingWidth);
            _isLeftPaneActualOpen = false;
            _isRightPaneActualOpen = true;
            _currentX = 0;
        }

        #endregion

    }
}