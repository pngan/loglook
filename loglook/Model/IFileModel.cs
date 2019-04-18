using System;
using System.Threading.Tasks;

namespace Model
{
    public interface IFileModel
    {
        string FilePath { get; }
        Task<int> GetLineCountAsync();
        void AddOrChangeSearchString(string searchString);
    }
}