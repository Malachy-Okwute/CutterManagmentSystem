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
        /// Command to navigate to updates page
        /// </summary>
        public ICommand NavigateToUpdatesPageCommand { get; set; }

        /// <summary>
        /// Command to navigate to archives page
        /// </summary>
        public ICommand NavigateToArchivesPageCommand { get; set; }

        /// <summary>
        /// Command to navigate to users page
        /// </summary>
        public ICommand NavigateToUsersPageCommand { get; set; }


        /// <summary>
        /// Command to navigate to settings page
        /// </summary>
        public ICommand NavigateToSettingsPageCommand { get; set; }

        /// <summary>
        /// Command to navigate to info page
        /// </summary>
        public ICommand NavigateToInfoPageCommand { get; set; }

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
            NavigateToUpdatesPageCommand = new RelayCommand(() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.UpdatePage));
            NavigateToArchivesPageCommand = new RelayCommand(() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.ArchivePage));
            NavigateToUsersPageCommand = new RelayCommand(() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.UserPage));
            NavigateToSettingsPageCommand = new RelayCommand(() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.SettingsPage));
            NavigateToInfoPageCommand = new RelayCommand(() => CurrentPage = _pageFactory.GetPageViewModel(AppPage.InformationPage));

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
