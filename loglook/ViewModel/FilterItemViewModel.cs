namespace ViewModel
{
    public class FilterItemViewModel : ViewModelBase, IFilterItemViewModel
    {
        public string Name => "FilterItemViewModel";
    }

    public interface IFilterItemViewModel
    {
        string Name { get; }
    }
}
