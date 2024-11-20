using System.Windows;
using System.Windows.Input;

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
            // Create commands
            MinimizeWindowCommand = new RelayCommand(() => _appWindow.WindowState = WindowState.Minimized, canExecuteCommand => true);
            //MaximizeWindowCommand = new RelayCommand(() => _appWindow.WindowState ^= WindowState.Maximized, canExecuteCommand => this != null);
            CloseWindowCommand = new RelayCommand(() => _appWindow.Close(), canExecuteCommand => true);
        }

        #endregion
    }
}
