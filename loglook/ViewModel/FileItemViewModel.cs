namespace ViewModel
{
    public class FileItemViewModel : ViewModelBase, IFileItemViewModel
    {
        public FileItemViewModel(IFilterListViewModel filterListViewModel, IGraphViewModel graphViewModel,  string path)
        {
            FilterListViewModel = filterListViewModel;
            GraphViewModel = graphViewModel;
            Path = path;
        }

        public IFilterListViewModel FilterListViewModel { get; }
        public IGraphViewModel GraphViewModel { get; }
        public string Path { get; }
    }

    public interface IFileItemViewModel
    {
        string Path { get; }
    }
}
