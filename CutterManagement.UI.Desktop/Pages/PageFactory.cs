using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public class PageFactory
    {
        private readonly Func<AppPage, ViewModelBase> _pageFactory;

        public PageFactory(Func<AppPage, ViewModelBase> factory)
        {
            _pageFactory = factory;
        }

        public ViewModelBase GetPageViewModel(AppPage page) => 
            _pageFactory.Invoke(page);
    }
}
