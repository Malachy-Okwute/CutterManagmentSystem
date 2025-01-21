using System.Windows;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="ApplicationWindow"/>
    /// </summary>
    public class ApplicationWindowViewModel : ViewModelBase
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
        private double _footerHeight = 24;

        /// <summary>
        /// The size of borders around the window to allow user to click and drag to resize the window
        /// </summary>
        private double _resizeBorderSize = 8;

        /// <summary>
        /// The main application window
        /// </summary>
        private Window _appWindow => Application.Current.MainWindow; //TODO: Refactor

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
        /// True if application window is maximized, otherwise false
        /// <remark>
        /// Used to switch maximize button icon between maximized and normal state of the window
        /// </remark>
        /// </summary>
        public bool IsMaximized {  get; set; }

        /// <summary>
        /// App navigation
        /// </summary>
        public NavigationBarViewModel Navigation { get; } //TODO: Refactor

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to minimize <see cref="ApplicationWindow"/>
        /// </summary>
        public ICommand MinimizeWindowCommand {  get; set; }

        /// <summary>
        /// Command to maximize <see cref="ApplicationWindow"/>
        /// </summary>
        //public ICommand MaximizeWindowCommand { get; set; }

        /// <summary>
        /// Command to close <see cref="ApplicationWindow"/>
        /// </summary>
        public ICommand CloseWindowCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationWindowViewModel(NavigationBarViewModel navigation)
        {
            // set app navigation
            Navigation = navigation;

            // Create commands
            CloseWindowCommand = new RelayCommand(() => _appWindow.Close());
            MinimizeWindowCommand = new RelayCommand(() => _appWindow.WindowState = WindowState.Minimized);
            //MaximizeWindowCommand = new RelayCommand(() => _appWindow.WindowState ^= WindowState.Maximized);
        }

        #endregion
    }
}
