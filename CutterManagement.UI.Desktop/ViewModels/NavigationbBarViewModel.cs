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
        public ViewModelBase CurrentPage { get; private set;}

        /// <summary>
        /// View model for <see cref="UserProfileControl"/>
        /// </summary>
        public ShiftProfileViewModel ShiftProfileViewModel { get; set; }

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
        public NavigationBarViewModel(PageFactory pageFactory, ShiftProfileViewModel shiftProfileViewModel)
        {
            // Set page factory
            _pageFactory = pageFactory;
            ShiftProfileViewModel = shiftProfileViewModel;

            // Create commands
            NavigateToHomePageCommand = new RelayCommand(GotoHomePage);
            NavigateToUpdatesPageCommand = new RelayCommand(() => 
            {
                var page = _pageFactory.GetPageViewModel(AppPage.UpdatePage);

                if (page.GetType() != CurrentPage?.GetType())
                {
                    CurrentPage = page;
                }
            });
            NavigateToArchivesPageCommand = new RelayCommand(() =>
            {
                var page = _pageFactory.GetPageViewModel(AppPage.ArchivePage);

                if (page.GetType() != CurrentPage?.GetType())
                {
                    CurrentPage = page;
                }
            });
            NavigateToUsersPageCommand = new RelayCommand(() =>
            {
                var page = _pageFactory.GetPageViewModel(AppPage.UserPage);

                if (page.GetType() != CurrentPage?.GetType())
                {
                    CurrentPage = page;
                }
            });
            NavigateToSettingsPageCommand = new RelayCommand(() => 
            {
                var page = _pageFactory.GetPageViewModel(AppPage.SettingsPage);

                if (page.GetType() != CurrentPage?.GetType())
                {
                    CurrentPage = page;
                }
            });
            NavigateToInfoPageCommand = new RelayCommand(() =>
            {
                var page = _pageFactory.GetPageViewModel(AppPage.InformationPage);

                if (page.GetType() != CurrentPage?.GetType())
                {
                    CurrentPage = page;
                }
            });

            // Goto home page on start up
            GotoHomePage();
        }

        #endregion

        /// <summary>
        /// Navigates to the home page of this application 
        /// </summary>
        private void GotoHomePage()
        {
            var page = _pageFactory.GetPageViewModel(AppPage.HomePage);

            if (CurrentPage is null || page.GetType() != CurrentPage.GetType())
            {
                CurrentPage = page;
            }
        }
    }
}
