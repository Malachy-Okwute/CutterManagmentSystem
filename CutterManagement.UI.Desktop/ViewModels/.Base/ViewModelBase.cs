using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model base for this application view models
    /// </summary>
    public class ViewModelBase : MarkupExtension, INotifyPropertyChanged
    {
        /// <summary>
        /// The event to fire when a property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Property changed callback
        /// </summary>
        /// <param name="propertyName">The property who's property has changed</param>
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // Invoke the property changed event
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
