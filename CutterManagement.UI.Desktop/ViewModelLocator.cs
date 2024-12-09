using System.ComponentModel;
using System.Windows;

namespace CutterManagement.UI.Desktop
{
    public class ViewModelLocator 
    {
        public static bool GetViewModelLocator(DependencyObject obj)
        {
            return (bool)obj.GetValue(ViewModelLocatorProperty);
        }

        public static void SetViewModelLocator(DependencyObject obj, bool value)
        {
            obj.SetValue(ViewModelLocatorProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewModelLocator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelLocatorProperty =
            DependencyProperty.RegisterAttached("ViewModelLocator", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, ViewModelChanged));

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(DesignerProperties.GetIsInDesignMode(d)) 
                return;
            string getViewModelName;
            Type view = d.GetType();

            if (view.ToString().EndsWith("Page"))
            {
                getViewModelName = (view).ToString().Replace("Page", "ViewModel", StringComparison.InvariantCulture);
            }
            else
            { 
                getViewModelName = (view).ToString().Replace("Control", "ViewModel", StringComparison.InvariantCulture);
            }

            Type? viewModelObjectType = Type.GetType(getViewModelName);

            if(viewModelObjectType != null) 
            {
                object? viewModel = Activator.CreateInstance(viewModelObjectType);
                ((FrameworkElement)d).DataContext = viewModel;
            }
        }
    }
}
