using System;
using System.Threading.Tasks;

namespace Model
{
    public interface IFileModel
    {
        event EventHandler<SeriesAddedOrChangedArgs> OnSeriesAddedOrChanged;

        string FilePath { get; }
        Task<int> GetLineCountAsync();
        Task AddOrChangeSearchString(int index, string searchString);
    }
}