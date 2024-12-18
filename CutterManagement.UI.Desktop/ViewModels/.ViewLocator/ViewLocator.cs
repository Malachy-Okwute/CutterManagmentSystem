using System.Windows;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Locates and present the appropriate view when view changes
    /// </summary>
    public class ViewLocator 
    {

        /*
         * Using attached property to locate and provide views
         */

        public static object GetFindView(DependencyObject obj)
        {
            return (object)obj.GetValue(FindViewProperty);
        }

        public static void SetFindView(DependencyObject obj, object value)
        {
            obj.SetValue(FindViewProperty, value);
        }

        // Using a DependencyProperty as the backing store for FindView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FindViewProperty =
            DependencyProperty.RegisterAttached("FindView", typeof(object), typeof(ViewLocator), new PropertyMetadata(OnPropertyChanged));

        /// <summary>
        /// Event that gets fired when view changes
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Make sure we have new value
            if(e.NewValue != e.OldValue)
            {
                // Use view model name to find view
                string viewName = e.NewValue.ToString()!.Replace("viewmodel", "", StringComparison.InvariantCultureIgnoreCase);

                // Get view type if it's available
                Type? viewType = Type.GetType(viewName);

                // Create an instance of the view
                FrameworkElement view = (FrameworkElement)Activator.CreateInstance(viewType ?? throw new ArgumentNullException())!;

                // Set view's data context
                view.DataContext = e.NewValue;

                // Inject view as content
                ((Frame)d).Content = view;
            }
        }
    }
}
