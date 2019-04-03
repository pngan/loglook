using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class FilterListViewModel : ViewModelBase, IFilterListViewModel
    {
        public string Name => "FilterListViewModel";
    }

    public interface IFilterListViewModel
    {
        string Name { get; }

    }
}
