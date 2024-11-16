using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml.XPath;

namespace CMS
{
    /// <summary>
    /// Interaction logic for CMSSplashWindow.xaml
    /// </summary>
    public partial class CMSSplashWindow : Window, INotifyPropertyChanged
    {
        public string CurrentTask { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged = (s, e) => 
        {
            new PropertyChangedEventArgs(nameof(CurrentTask));
        };

        public CMSSplashWindow()
        {
            InitializeComponent();

            DataContext = this;

            CurrentTask = string.Empty;
        }

    }
}
