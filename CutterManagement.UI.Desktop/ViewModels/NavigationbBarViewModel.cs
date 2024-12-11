using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="NavigationBarControl"/>
    /// </summary>
    public class NavigationBarViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Application page factory
        /// </summary>
        private readonly PageFactory _pageFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Page that is currently showing in the view
        /// </summary>
        public ViewModelBase? CurrentPage { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to navigate to home page
        /// </summary>
        public ICommand NavigateToHomePageCommand { get; set; }

        /// <summary>
        /// Command to navigate to settings page
        /// </summary>
        public ICommand NavigateToSettingsPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="pageFactory">Page factory</param>
        public NavigationBarViewModel(PageFactory pageFactory)
        {
            // Set page factory
            _pageFactory = pageFactory;

            // Create commands
            NavigateToHomePageCommand = new RelayCommand(GotoHomePage);
            NavigateToSettingsPageCommand = new RelayCommand(() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.SettingsPage));

            // Goto home page on start up
            GotoHomePage();
        }

        #endregion


        /// <summary>
        /// Navigates to the home page of this application 
        /// </summary>
        private void GotoHomePage() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.HomePage);
        
    }
}
