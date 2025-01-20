namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="HomePage"/>
    /// </summary>
    public class HomePageViewModel : ViewModelBase
    {
        /// <summary>
        /// List of machine items
        /// </summary>
        public MachineItemCollectionViewModel MachineItemCollection { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineItemCollection">List of machine items</param>
        public HomePageViewModel(MachineItemCollectionViewModel machineItemCollection)
        {
            MachineItemCollection = machineItemCollection;
        }
    }
}
