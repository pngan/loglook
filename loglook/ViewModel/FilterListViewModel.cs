using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class FilterListViewModel : ViewModelBase, IFilterListViewModel
    {
        private readonly IFileModel m_fileModel;

        public FilterListViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
        }
        public string Name => "FilterListViewModel";
    }

    public interface IFilterListViewModel
    {
        string Name { get; }

    }
}
