using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public class InfoPageViewModel : ViewModelBase
    {
        public string Message { get; set; } = $"This Page is currently under development. " +
                                              $"{Environment.NewLine}We are working hard to bring you this feature soon. " +
                                              $"{Environment.NewLine}Please check back later";
    }
}
