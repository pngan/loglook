using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class FileRawViewModel : ViewModelBase, IFileRawViewModel
    {
        public string Name => "FileRawViewModel";
    }

    public interface IFileRawViewModel
    {
        string Name { get; }

    }
}
