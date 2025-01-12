using CutterManagement.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for AdminLoginDialog.xaml
    /// </summary>
    public partial class AdminLoginDialog : UserControl, ISecurePassword
    {
        public AdminLoginDialog()
        {
            InitializeComponent();
        }

        public SecureString Password => CustomPassWordControl.PasswordControl.SecurePassword;

        public void Dispose()
        {
            try
            {
                CustomPassWordControl.PasswordControl.Clear();
                CustomPassWordControl.PasswordControl.SecurePassword.Dispose();
                Password.Clear();
                Password.Dispose();
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(ex.Message);
            }
            
        }
    }
}
