using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CMSWindow
    {
        #region Private Snap Layout Constants

        /// <summary>
        /// Window message for screen coordinates
        /// </summary>
        private const int WM_NCHITTEST = 0x0084;

        /// <summary>
        /// Window message for button down / button being pressed
        /// </summary>
        private const int WM_NCLBUTTONDOWN = 0x00A1;

        /// <summary>
        /// Window snap layout message
        /// </summary>
        private const int HTMAXBUTTON = 9;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods (Window Snap Layout Popup)

        /// <summary>
        /// Override the <see cref="OnSourceInitialized(EventArgs)"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            // Let base do what it needs
            base.OnSourceInitialized(e);

            // Monitor window messages
            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(new HwndSourceHook(WndProc));
        }

        /// <summary>
        /// Get mouse pointer location
        /// </summary>
        /// <param name="lParam">The x and y values</param>
        /// <returns>Mouse pointer coordinates</returns>
        private Point GetCursorPosition(IntPtr lParam)
        {
            int x = lParam.ToInt32() & 0xFFFF;
            int y = lParam.ToInt32() >> 16;

            return new Point(x, y);
        }

        /// <summary>
        /// Checks the location of mouse pointer
        /// </summary>
        /// <param name="cursorPos">Mouse pointer coordinate</param>
        /// <returns>True if mouse pointer is over maximize button, otherwise false</returns>
        private bool IsCursorOverMaximizeButton(Point cursorPos)
        {
            // Gets the location of maximize button
            Point buttonLocation = MaximizeButton.PointToScreen(new Point());

            // Check if mouse pointer is over maximize button on x axis
            if (cursorPos.X > (buttonLocation.X + MaximizeButton.ActualWidth) || cursorPos.X < buttonLocation.X)
            {
                // Set button background and foreground manually
                MaximizeButton.Background = (Brush)TryFindResource("BackgroundColorBrush1");
                // Return false
                return false;
            }

            // Check if mouse pointer is over maximize button on y axis
            if (cursorPos.Y > (buttonLocation.Y + MaximizeButton.ActualHeight) || cursorPos.Y < buttonLocation.Y)
            {
                // Set button background and foreground manually
                MaximizeButton.Background = (Brush)TryFindResource("BackgroundColorBrush1");
                // Return false
                return false;
            }

            // Return true if conditions are met
            return true;
        }

        /// <summary>
        /// Window procedure messages
        /// </summary>
        /// <param name="msg">Window messages</param>
        /// <param name="lParam">Mouse pointer coordinate</param>
        /// <param name="handled">Flag for if operation is handled or not handled</param>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if ((nint)msg == WM_NCHITTEST)
            {
                // Calculate the position of the cursor
                Point cursorPos = GetCursorPosition(lParam);

                // Make sure mouse pointer is over maximize button
                if (IsCursorOverMaximizeButton(cursorPos))
                {
                    MaximizeButton.Background = (Brush)TryFindResource("BackgroundColorBrush3");
                    handled = true;
                    return HTMAXBUTTON;
                }
            }
            // Respond to when user clicks on the maximize button
            else if(msg == WM_NCLBUTTONDOWN)
            {
                // Set button background and foreground manually
                MaximizeButton.Background = (Brush)TryFindResource("BackgroundColorBrush1");

                // Maximize window or set it to normal
                WindowState ^= WindowState.Maximized;
            }
            else
                handled = false;

            // Do nothing else
            return IntPtr.Zero;
        }

        #endregion
    }
}