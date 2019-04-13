namespace ViewModel
{
    public class FilterItemViewModel : ViewModelBase, IFilterItemViewModel
    {
        public IFilterItemViewModel FileItemViewModel { get; }
        public IGraphViewModel GraphViewModel { get; }

        public FilterItemViewModel(IFilterItemViewModel fileItemViewModel, IGraphViewModel graphViewModel)
        {
            FileItemViewModel = fileItemViewModel;
            GraphViewModel = graphViewModel;
        }
    }

    public interface IFilterItemViewModel
    {
        IFilterItemViewModel FileItemViewModel { get; }
        IGraphViewModel GraphViewModel { get; }
    }
}
