using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Custom window for this application 
    /// </summary>
    public class ApplicationWindow : Window
    {
        #region Private Field

        /// <summary>
        /// Window handle
        /// </summary>
        private IntPtr _handle;

        /// <summary>
        /// Cursor offset
        /// </summary>
        private Point _cursorOffset;

        /// <summary>
        /// Previous <see cref="Window.Left"/>
        /// </summary>
        private double _windowPreviousLeft;

        /// <summary>
        /// Previous <see cref="Window.Height"/>
        /// </summary>
        private double _windowPreviousHeight;

        /// <summary>
        /// Previous widow width
        /// </summary>
        //private double _windowPreviousWidth;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationWindow(ApplicationWindowViewModel viewModel)
        {
            // Get view model of this window 
            DataContext = viewModel;

            // Win32 operation event
            SourceInitialized += (s, e) => _handle = new WindowInteropHelper(this).Handle;

            // Apply styling to this window
            Style = (Style)TryFindResource("CMSWindowStyle");

            // Screen work area
            Rect workArea = SystemParameters.WorkArea;

            // Hook into window size changed event
            SizeChanged += (s, e) =>
            {
                // Set view model properties
                viewModel.IsMaximized = WindowState == WindowState.Maximized;
                viewModel.ResizeBorderSize = WindowState == WindowState.Maximized ? 0 : 8;

                // If window is in maximized state do nothing
                if (WindowState == WindowState.Maximized) return;

                #region Snap Layout

                // If window edge is on the left | top left | bottom left of the screen...
                if ((Left == workArea.Left && Top == workArea.Top) || (Left == workArea.Left && (Height + Top) == workArea.Bottom))
                {
                    // Disable drop shadow
                    viewModel.DropShadowPadding = 0;
                }
                // If window edge is on the right | top right | bottom right of the screen...
                else if ((Left + Width == workArea.Right && Top == workArea.Top) || ((Left + Width) == workArea.Right && (Height + Top) == workArea.Bottom))
                {
                    // Disable drop shadow
                    viewModel.DropShadowPadding = 0;
                }
                // If window edge is on the top and bottom of the screen...
                else if ((Top == workArea.Top) && (Height == workArea.Bottom))
                {
                    // Disable drop shadow
                    viewModel.DropShadowPadding = 0;
                }
                // Otherwise
                else
                {
                    // Set drop shadow and resize border sizes
                    viewModel.DropShadowPadding = 40;
                    viewModel.ResizeBorderSize = 8;
                }

                #endregion
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Template to use for this window
        /// </summary>
        public override void OnApplyTemplate()
        {
            // Flag used to track when window is maximized
            bool isWindowMaximized = false;

            // Get template elements
            FrameworkElement captionArea = (FrameworkElement)GetTemplateChild("PART_CaptionArea");
            FrameworkElement windowLimitMargin = (FrameworkElement)GetTemplateChild("PART_WindowLimitMargin");

            // caption area element isn't null
            if (captionArea != null)
            {
                // Hook into left mouse down event
                captionArea.MouseLeftButtonDown += (sender, e) =>
                {
                    _windowPreviousLeft = Left;

                    // check if user double clicked the title bar
                    if (e.ClickCount.Equals(2))
                        // Switch between normal and maximized states when user double clicks the title bar.
                        WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                    // Otherwise...
                    else
                    {
                        // If window is maximized
                        if (WindowState == WindowState.Maximized)
                            // Set flag to true
                            isWindowMaximized = true;

                        if (Top == SystemParameters.WorkArea.Top && (Left + ActualWidth) == SystemParameters.WorkArea.Right || (Left + ActualWidth) == SystemParameters.WorkArea.Left)
                        {
                            // Make sure left mouse button is pressed
                            if (Mouse.LeftButton == MouseButtonState.Pressed)
                            {
                                // Get mouse location 
                                double mousePos = e.GetPosition(this).Y;

                                // Set location 
                                Left = _windowPreviousLeft - 54;
                                Top = mousePos - 15;  
                                Height = _windowPreviousHeight;
                            }
                        }
                        // If window is extended to max vertically
                        else if (Top == SystemParameters.WorkArea.Top)
                        {
                            // Make sure left mouse button is pressed
                            if (Mouse.LeftButton == MouseButtonState.Pressed)
                            {
                                // Get mouse location 
                                double mousePos = e.GetPosition(this).Y;

                                // Set window location 
                                Left = _windowPreviousLeft;
                                Top = mousePos - 54; // Account for drop shadow and margin | padding 
                                Height = _windowPreviousHeight;
                            }
                        }

                        // Drag window
                        DragMove();
                    }

                    // Reset isWindowMaximized flag when left mouse button is up
                    captionArea.MouseLeftButtonUp += (sender, e) => { isWindowMaximized = false; };

                    // Hook into mouse move event
                    captionArea.MouseMove += (sender, e) =>
                    {
                        // If window is in a maximized state
                        if (isWindowMaximized)
                        {
                            // Reset flag
                            isWindowMaximized = false;

                            // Get mouse position 
                            double mousePos = e.GetPosition(this).X;
                            // Get bounds width of previous window location
                            double restoreBoundsWidth = RestoreBounds.Width;
                            double newWindowPos = mousePos - restoreBoundsWidth / 2;

                            if (newWindowPos < 0)
                                newWindowPos = 0;
                            else if (newWindowPos + restoreBoundsWidth > SystemParameters.WorkArea.Width)
                                newWindowPos = SystemParameters.WorkArea.Width - restoreBoundsWidth;

                            // Restore window
                            WindowState = WindowState.Normal;

                            // Set window new location
                            Left = newWindowPos;
                            Top = -((ApplicationWindowViewModel)DataContext).DropShadowPadding; // Take into account the size of padding around this window

                            // Make sure left mouse button is pressed
                            if(Mouse.LeftButton == MouseButtonState.Pressed)
                                // Move window 
                                DragMove();
                        }

                    };

                };
            }

            // If we have window limit...
            if (windowLimitMargin != null)
            {
                // Monitor when size of this window changes
                SizeChanged += (sender, e) =>
                {
                    // If window is maximized...
                    if (WindowState == WindowState.Maximized)
                        // Fix over-sized window size
                        windowLimitMargin.Margin = GetMaximizedMarginThickness();
                    // Otherwise
                    else
                        // Reset margin to nothing
                        windowLimitMargin.Margin = new Thickness(0);
                };
            }

            // Show the appropriate cursor around window border for / during window resize
            GetResizeBorders();
        }

        /// <summary>
        /// Calculates the right margin value, used to keep this window from being too large
        /// </summary>
        /// <returns><see cref=" Thickness"/></returns>
        private Thickness GetMaximizedMarginThickness()
        {
            FrameworkElement windowLimitMargin = (FrameworkElement)GetTemplateChild("PART_WindowLimitMargin");
            if (windowLimitMargin == null)
                return new Thickness(0);

            Rect area = SystemParameters.WorkArea;
            Point detectorCorner = windowLimitMargin.PointToScreen(new Point(0, 0));

            PresentationSource presentationSource = PresentationSource.FromVisual(this);
            double dpiScaleX = presentationSource.CompositionTarget.TransformToDevice.M11;
            double dpiScaleY = presentationSource.CompositionTarget.TransformToDevice.M22;

            double areaRight = area.Width / dpiScaleX;
            double areaBottom = area.Height / dpiScaleY;
            double offsetX = area.Left - detectorCorner.X;
            double offsetY = area.Top - detectorCorner.Y;

            double top = offsetX / dpiScaleX;
            double left = offsetY / dpiScaleY;
            double right = (windowLimitMargin.ActualWidth - offsetX - areaRight) / dpiScaleX;
            double bottom = (windowLimitMargin.ActualHeight - offsetY - areaBottom) / dpiScaleY;

            return new Thickness(left, top, right, bottom);
        }

        /// <summary>
        /// Get window resize border elements
        /// </summary>
        public void GetResizeBorders()
        {
            // Get template elements
            FrameworkElement leftBorder = (FrameworkElement)GetTemplateChild("PART_LeftBorder");
            FrameworkElement rightBorder = (FrameworkElement)GetTemplateChild("PART_RightBorder");
            FrameworkElement topBorder = (FrameworkElement)GetTemplateChild("PART_TopBorder");
            FrameworkElement bottomBorder = (FrameworkElement)GetTemplateChild("PART_BottomBorder");
            FrameworkElement topLeftBorder = (FrameworkElement)GetTemplateChild("PART_TopLeftBorder");
            FrameworkElement topRightBorder = (FrameworkElement)GetTemplateChild("PART_TopRightBorder");
            FrameworkElement bottomLeftBorder = (FrameworkElement)GetTemplateChild("PART_BottomLeftBorder");
            FrameworkElement bottomRightBorder = (FrameworkElement)GetTemplateChild("PART_BottomRightBorder");

            // Map borders events
            ResizeBorderEvents(WindowBorder.Left, leftBorder);
            ResizeBorderEvents(WindowBorder.Right, rightBorder);
            ResizeBorderEvents(WindowBorder.Top, topBorder);
            ResizeBorderEvents(WindowBorder.Bottom, bottomBorder);
            ResizeBorderEvents(WindowBorder.TopLeft, topLeftBorder);
            ResizeBorderEvents(WindowBorder.TopRight, topRightBorder);
            ResizeBorderEvents(WindowBorder.BottomLeft, bottomLeftBorder);
            ResizeBorderEvents(WindowBorder.BottomRight, bottomRightBorder);
        }

        /// <summary>
        /// Hook up border mouse events
        /// </summary>
        /// <param name="edge">The location of the border on this window</param>
        /// <param name="border">The actual border</param>
        private void ResizeBorderEvents(WindowBorder edge, FrameworkElement border)
        {
            // Hook up mouse enter event
            border.MouseEnter += (sender, e) =>
            {
                // Make sure window is not maximized
                if (WindowState != WindowState.Maximized)
                {
                    // Assign appropriate cursor to the border that is being hovered
                    switch (edge)
                    {
                        case WindowBorder.Left:
                        case WindowBorder.Right:
                            border.Cursor = Cursors.SizeWE;
                            break;

                        case WindowBorder.Top:
                        case WindowBorder.Bottom:
                            border.Cursor = Cursors.SizeNS;
                            break;

                        case WindowBorder.TopLeft:
                        case WindowBorder.BottomRight:
                            border.Cursor = Cursors.SizeNWSE;
                            break;

                        case WindowBorder.TopRight:
                        case WindowBorder.BottomLeft:
                            border.Cursor = Cursors.SizeNESW;
                            break;
                    }
                }
                else
                    border.Cursor = Cursors.Arrow;
            };

            // Hook up left mouse button down event
            border.MouseLeftButtonDown += (sender, e) =>
            {
                // Make sure window is not maximized
                if (WindowState != WindowState.Maximized)
                {
                    // Get location of this window
                    Point cursorLocation = e.GetPosition(this);
                    // Cursor offset variable
                    Point cursorOffset = new Point();

                    // Set the appropriate location for the selected border
                    switch(edge)
                    {
                        case WindowBorder.Left:
                            cursorOffset.X = cursorLocation.X;
                            break;

                        case WindowBorder.TopLeft:
                            cursorOffset.Y = cursorLocation.Y;
                            cursorOffset.X = cursorLocation.X;
                            break;

                        case WindowBorder.Top:
                            cursorOffset.Y = cursorLocation.Y;
                            break;

                        case WindowBorder.TopRight:
                            cursorOffset.X = Width - cursorLocation.X;
                            cursorOffset.Y = cursorLocation.Y;
                            break;

                        case WindowBorder.Right:
                            cursorOffset.X = Width - cursorLocation.X;
                            break;

                        case WindowBorder.BottomRight:
                            cursorOffset.X = Width - cursorLocation.X;
                            cursorOffset.Y = Height - cursorLocation.Y;
                            break;

                        case WindowBorder.Bottom:
                            cursorOffset.Y = Height - cursorLocation.Y;
                            break;

                        case WindowBorder.BottomLeft:
                            cursorOffset.X = cursorLocation.X;
                            cursorOffset.Y = Height - cursorLocation.Y;
                            break;
                    }

                    // Set cursor offset
                    _cursorOffset = cursorOffset;

                    // If window is not maximized and window height is not at max
                    if(Top != SystemParameters.WorkArea.Top && WindowState != WindowState.Maximized)
                    {
                        // Capture windows Left and height values
                        _windowPreviousLeft = Left;
                        _windowPreviousHeight = Height;
                    }

                    // Capture mouse
                    border.CaptureMouse();
                }
            };

            // Hook up mouse move event
            border.MouseMove += (sender, e) =>
            {
                // Make sure window is not maximized and mouse is captured on the border selected 
                if (WindowState != WindowState.Maximized && border.IsMouseCaptured)
                {
                    // Get window location
                    Point cursorLocation = e.GetPosition(this);

                    // Horizontal and vertical change
                    double positiveHorizontalChange = cursorLocation.X + _cursorOffset.X;
                    double negativeHorizontalChange = cursorLocation.X - _cursorOffset.X;
                    double positiveVerticalChange = cursorLocation.Y + _cursorOffset.Y;
                    double negativeVerticalChange = cursorLocation.Y - _cursorOffset.Y;

                    // Move and adjust window border and size using the border selected
                    switch(edge)
                    {
                        case WindowBorder.Left:
                            if (Width - negativeHorizontalChange <= MinWidth)
                                break;
                            Width -= negativeHorizontalChange;
                            Left += negativeHorizontalChange;
                            break;

                        case WindowBorder.TopLeft:
                            if (Width - negativeHorizontalChange <= MinWidth)
                                break;

                            Width -= negativeHorizontalChange;
                            Left += negativeHorizontalChange;

                            if (Height - negativeVerticalChange <= MinHeight)
                                break;

                            Height -= negativeVerticalChange;
                            Top += negativeVerticalChange;
                            break;

                        case WindowBorder.Top:
                            if (Height - negativeVerticalChange <= MinHeight)
                                break;

                            Height -= negativeVerticalChange;
                            Top += negativeVerticalChange;
                            break;

                        case WindowBorder.TopRight:
                            if (positiveHorizontalChange <= MinWidth)
                                break;

                            Width = positiveHorizontalChange;
                            if (Height - negativeVerticalChange <= MinHeight)
                                break;

                            Height -= negativeVerticalChange;
                            Top += negativeVerticalChange;
                            break;

                        case WindowBorder.Right:
                            if(positiveHorizontalChange <= MinWidth)
                                break;

                            Width = positiveHorizontalChange;
                            break;

                        case WindowBorder.BottomRight:
                            if (positiveHorizontalChange <= MinWidth)
                                break;

                            Width = positiveHorizontalChange;
                            if(positiveVerticalChange <= MinHeight)
                                break;

                            Height = positiveVerticalChange;
                            break;

                        case WindowBorder.Bottom:
                            if (positiveVerticalChange <= MinHeight)
                                break;
                            if (Height + Top - 20 > SystemParameters.WorkArea.Height)
                                break;

                            Height = positiveVerticalChange;
                            break;

                        case WindowBorder.BottomLeft:
                            if(Width -  negativeHorizontalChange <= MinWidth)
                                break;

                            Width -= negativeHorizontalChange;
                            Left += negativeHorizontalChange;

                            if (positiveVerticalChange <= MinHeight)
                                break;

                            Height = positiveVerticalChange;
                            break;
                    }
                }
            };

            // Release mouse capture
            border.MouseLeftButtonUp += (sender, e) =>
            {
                border.ReleaseMouseCapture();

                // If window is already at the top or bottom
                if ((Top + ((ApplicationWindowViewModel)DataContext).DropShadowPadding) < SystemParameters.WorkArea.Top ||
                (Height + Top - 20 > SystemParameters.WorkArea.Height))
                {
                    // Extend window to max vertically
                    Top = SystemParameters.WorkArea.Top;
                    Height = SystemParameters.WorkArea.Height;
                }
            };
        }

        #endregion

        /// <summary>
        /// Window border edge
        /// </summary>
        private enum WindowBorder
        {
            Left, Top, Right, Bottom, TopLeft, TopRight, BottomLeft, BottomRight,
        }
    }
}
