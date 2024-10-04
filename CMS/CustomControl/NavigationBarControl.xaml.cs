using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CMS
{
    /// <summary>
    /// Interaction logic for NavigationBarControl.xaml
    /// </summary>
    public partial class NavigationBarControl : UserControl
    {
        /// <summary>
        /// Name of selected button
        /// <remark>Defaulted to home button</remark>
        /// </summary>
        private string _selectedButtonName = nameof(HomeButton);

        /// <summary>
        /// Default constructor
        /// </summary>
        public NavigationBarControl()
        {
            InitializeComponent();

            // Set home button as selected by default
            Loaded += (sender, e) => SetMenuSelection(HomeButton);

            // Reset selected button whenever navigation bar changes it's size
            SizeChanged += (sender, e) =>
            {
                if (_selectedButtonName.Equals(HomeButton.Name))
                    SetMenuSelection(HomeButton);
                else if (_selectedButtonName.Equals(UpdatesButton.Name))
                    SetMenuSelection(UpdatesButton);
                else if (_selectedButtonName.Equals(ArchivesButton.Name))
                    SetMenuSelection(ArchivesButton);
                else if (_selectedButtonName.Equals(UsersButton.Name))
                    SetMenuSelection(UsersButton);
                else if (_selectedButtonName.Equals(SettingsButton.Name))
                    SetMenuSelection(SettingsButton);
                else if (_selectedButtonName.Equals(InfoButton.Name))
                    SetMenuSelection(InfoButton);
            };

            // Menu button selection event
            HomeButton.Click += OnMenuButtonSelected;
            UpdatesButton.Click += OnMenuButtonSelected;
            ArchivesButton.Click += OnMenuButtonSelected;
            UsersButton.Click += OnMenuButtonSelected;
            SettingsButton.Click += OnMenuButtonSelected;
            InfoButton.Click += OnMenuButtonSelected;
        }

        /// <summary>
        /// Menu button selection event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void OnMenuButtonSelected(object sender, RoutedEventArgs e)
        {
            // Make sure the selected button isn't null
            if (sender is not Button) return;

            // Get the name of the currently selected button
            _selectedButtonName = ((Button)sender).Name;
            
            // Sets menu button indicator to the selected button
            SetMenuSelection((Button)sender);
        }

        /// <summary>
        /// Locates and set menu selection indicator on the selected menu button
        /// </summary>
        /// <param name="button">The button to set the indicator to</param>
        private void SetMenuSelection(Button button)
        {
            // Get location of the selected menu button
            Point buttonLocation = button.TransformToAncestor(this).Transform(new Point());

            // Set button indicator
            Canvas.SetLeft(Indicator, buttonLocation.X);
        }
    }
}
