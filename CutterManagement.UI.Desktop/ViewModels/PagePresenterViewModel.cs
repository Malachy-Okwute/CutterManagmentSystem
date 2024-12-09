using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public class PagePresenterViewModel : ViewModelBase
    {
        public AppPage CurrentPage { get; set; } = AppPage.HomePage;

    }
}
