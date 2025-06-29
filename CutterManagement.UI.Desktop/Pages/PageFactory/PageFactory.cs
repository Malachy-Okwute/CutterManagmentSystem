namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Factory that provides pages of this application
    /// </summary>
    public class PageFactory
    {
        /// <summary>
        /// Function to run and get application page from.
        /// </summary>
        private readonly Func<AppPage, ViewModelBase> _pageFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="factory">The injection function to get page from</param>
        public PageFactory(Func<AppPage, ViewModelBase> factory)
        {
            _pageFactory = factory;
        }

        /// <summary>
        /// Gets an application page
        /// </summary>
        /// <param name="page">The type of page to get</param>
        /// <returns><see cref="ViewModelBase"/> object type</returns>
        public ViewModelBase GetPageViewModel(AppPage page) => _pageFactory.Invoke(page);
    }
}
