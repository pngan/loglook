using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class FileWindowViewModel : IFileWindowViewModel
    {
        public IFileFilteredViewModel FileFilteredViewModel { get; }
        public IFileRawViewModel FileRawViewModel { get; }
        public IFilterListViewModel FilterListViewModel { get; }
        public FileWindowViewModel(IFileFilteredViewModel fileFilteredViewModel, IFileRawViewModel fileRawViewModel, IFilterListViewModel filterListViewModel)
        {
            FileFilteredViewModel = fileFilteredViewModel;
            FileRawViewModel = fileRawViewModel;
            FilterListViewModel = filterListViewModel;
        }
    }

    public interface IFileWindowViewModel
    {

    }
}
