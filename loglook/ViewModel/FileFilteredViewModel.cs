namespace ViewModel
{
    public class FileFilteredViewModel : ViewModelBase, IFileFilteredViewModel
    {
        public string Name => "FileFilteredViewModel";
    }

    public interface IFileFilteredViewModel
    {
        string Name { get; }
    }
}
