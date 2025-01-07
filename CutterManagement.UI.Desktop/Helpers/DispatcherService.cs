using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace CutterManagement.UI.Desktop
{
    public static class DispatcherService 
    {
        public static void Invoke(Action action)
        {
            Dispatcher dispatchObject = Application.Current.Dispatcher;

            try
            {
                if (dispatchObject == null || dispatchObject.CheckAccess())
                {
                    action();
                }
                else
                {
                    dispatchObject.Invoke(action);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
            finally
            {
            }
        }
    }
}
