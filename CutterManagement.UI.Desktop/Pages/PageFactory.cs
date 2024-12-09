using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public class PageFactory
    {
        private readonly Func<AppPage, PagePresenterViewModel> _pageFactory;

        public PageFactory(Func<AppPage, PagePresenterViewModel> factory)
        {
            _pageFactory = factory;
        }

        public PagePresenterViewModel GetPageViewModel(AppPage page) => _pageFactory.Invoke(page);
    }
}
