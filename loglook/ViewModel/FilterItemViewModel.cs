using Model;

namespace ViewModel
{
    public class FilterItemViewModel : ViewModelBase, IFilterItemViewModel
    {
        private readonly IFileModel m_fileModel;
        private string m_searchString;
        private bool m_isVisible = true;

        public FilterItemViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
            m_searchString = fileModel.FilePath;
            MatchCount = 0;
        }

        public string SearchString
        {
            get => m_searchString;
            set => SetField(ref m_searchString, value);
        }

        public bool IsVisible
        {
            get => m_isVisible;
            set => SetField(ref m_isVisible, value);
        }

        public int MatchCount { get; }
    }

    public interface IFilterItemViewModel
    {
        string SearchString { get; set; }
        bool IsVisible { get; set; }
        int MatchCount { get; }
    }
}