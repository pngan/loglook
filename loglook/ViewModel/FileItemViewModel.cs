namespace ViewModel
{
    public class FileItemViewModel : ViewModelBase, IFileItemViewModel
    {
        public FileItemViewModel(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }

    public interface IFileItemViewModel
    {
        string Path { get; }
    }
}
