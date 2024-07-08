using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;

namespace CMS
{
    /// <summary>
    /// View model for <see cref="CMSWindow"/>
    /// </summary>
    public class CMSWindowViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// size of caption height of this window
        /// </summary>
        private double _captionHeight = 35;

        /// <summary>
        /// Padding around the window to show drop-shadow
        /// </summary>
        private double _dropShadowPadding = 40;

        /// <summary>
        /// The size of padding of contents of the window
        /// </summary>
        private double _innerPadding = 4;

        /// <summary>
        /// The height of footer of this application 
        /// </summary>
        private double _footerHeight = 22;

        /// <summary>
        /// The size of borders around the window to allow user to click and drag to resize the window
        /// </summary>
        private double _resizeBorderSize = 8;

        #endregion

        #region Public Properties

        /// <summary>
        /// size of caption height of this window
        /// </summary>
        public double CaptionHeight
        {
            get => _captionHeight;
            set => _captionHeight = value;
        }

        /// <summary>
        /// Padding around the window to show drop-shadow
        /// </summary>
        public double DropShadowPadding
        {
            get => _dropShadowPadding;
            set => _dropShadowPadding = value;
        }

        /// <summary>
        /// The size of padding of contents of the window
        /// </summary>
        public double InnerPadding
        {
            get => _innerPadding;
            set => _innerPadding = value;
        }

        /// <summary>
        /// The size of borders around the window to allow user to click and drag to resize the window
        /// </summary>
        public double ResizeBorderSize
        {
            get => _resizeBorderSize; 
            set => _resizeBorderSize = value;
        }

        /// <summary>
        /// The height of footer of this application 
        /// </summary>
        public double FooterHeight
        {
            get => _footerHeight; 
            set => _footerHeight = value;
        }

        /// <summary>
        /// True if <see cref="CMSWindow"/> is maximized, 
        /// otherwise false
        /// </summary>
        public bool IsMaximized { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to minimize <see cref="CMSWindow"/>
        /// </summary>
        public ICommand MinimizeWindowCommand {  get; set; }

        /// <summary>
        /// Command to maximize <see cref="CMSWindow"/>
        /// </summary>
        //public ICommand MaximizeWindowCommand { get; set; }

        /// <summary>
        /// Command to close <see cref="CMSWindow"/>
        /// </summary>
        public ICommand CloseWindowCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSWindowViewModel()
        {
            // Get app window
            Window cmsWindow = Application.Current.MainWindow;

            // Screen work area
            Rect workArea = SystemParameters.WorkArea;

            // Hook into window size changed event
            cmsWindow.SizeChanged += (s, e) =>
            {
                // Set is-maximized to true if window is maximized
                IsMaximized = cmsWindow.WindowState == WindowState.Maximized;

                // Set resize border with to 0 when window is maximized and 8 if window is not maximized
                //ResizeBorderSize = cmsWindow.WindowState == WindowState.Maximized ? 0 : 8;

                #region Snap Layout

                // The new size layout to snap this window to
                Size newSize = e.NewSize;

                // Snap to left or right equal half of the snap layout
                if (newSize.Width == workArea.Width / 2 && newSize.Height == workArea.Height)
                    DropShadowPadding = 0;
                // Snap to either of the 4 equal sections of the snap layout corners
                else if (newSize.Width == workArea.Width / 2 && newSize.Height == workArea.Height / 2)
                    DropShadowPadding = 0;
                // Snap to either of the 3 equal sections of the snap layout
                else if (newSize.Width == workArea.Width / 3 && newSize.Height == workArea.Height)
                    DropShadowPadding = 0;

                // Snap to center of the 3 unequal sections of the snap layout
                else if (newSize.Width == GetSnapLayoutSectionSizeUsingPercentageValue(workArea.Width, 43.958) && newSize.Height == workArea.Height)
                    DropShadowPadding = 0;
                // Snap to either sides of the 3 unequal sections of the snap layout
                else if (newSize.Width == GetSnapLayoutSectionSizeUsingPercentageValue(workArea.Width, 28.021) && newSize.Height == workArea.Height)
                    DropShadowPadding = 0;
                // Snap to the 2/3 of section of the snap layout
                else if (newSize.Width == (workArea.Width / 3) * 2 && newSize.Height == workArea.Height)
                    DropShadowPadding = 0;
                // Otherwise
                else
                {
                    DropShadowPadding = 40;
                    ResizeBorderSize = 8;
                }

                #endregion
            };

            // Create commands
            MinimizeWindowCommand = new RelayCommand(() => cmsWindow.WindowState = WindowState.Minimized, canExecuteCommand => this != null);
            //MaximizeWindowCommand = new RelayCommand(() => cmsWindow.WindowState ^= WindowState.Maximized, canExecuteCommand => this != null);
            CloseWindowCommand = new RelayCommand(cmsWindow.Close, canExecuteCommand => this != null);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the actual width of a section of window layout to snap to using the supplied percentage value
        /// </summary>
        /// <param name="maxWorkAreaSize">The max work area size</param>
        /// <param name="percentageValue">The percentage value to use</param>
        /// <returns>Number value</returns>
        private double GetSnapLayoutSectionSizeUsingPercentageValue(double maxWorkAreaSize, double percentageValue) => Math.Round((percentageValue / 100) * maxWorkAreaSize);

        #endregion
    }
}
