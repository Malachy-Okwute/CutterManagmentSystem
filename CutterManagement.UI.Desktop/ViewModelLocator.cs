using Microsoft.Extensions.DependencyInjection;
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

            Type view = d.GetType();

            if(view.FullName is not null)
            {
                string getViewModelName = view.FullName.Insert(view.FullName.Length, "ViewModel");

                Type? viewModelObjectType = Type.GetType(getViewModelName);

                if(viewModelObjectType is not null && ((FrameworkElement)d).DataContext is null) 
                {
                    object? viewModel = Activator.CreateInstance(viewModelObjectType);
                    ((FrameworkElement)d).DataContext = viewModel;
                }
            }
        }
    }
}
