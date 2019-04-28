using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Threading;
using Model;

namespace ViewModel
{
    public class FilterItemViewModel : ViewModelBase, IFilterItemViewModel
    {
        private readonly int m_index;
        private readonly IFileModel m_fileModel;
        private readonly IGraphViewModel m_graphViewModel;
        private string m_searchString;
        private bool m_isVisible = true;
        private readonly Subject<string> m_stringSubject = new Subject<string>();
        private int m_matchCount;
        private bool m_isReservedSearchString;

        public FilterItemViewModel(int index, IFileModel fileModel, IGraphViewModel graphViewModel)
        {
            m_index = index;
            m_fileModel = fileModel;
            m_graphViewModel = graphViewModel;
            MatchCount = 0;
            m_stringSubject.AsObservable()
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {
                    m_fileModel.AddOrChangeSearchString(m_index, x);
                });
            m_fileModel.OnSeriesAddedOrChanged += FileModelOnOnSeriesAddedOrChanged;

        }

        private void FileModelOnOnSeriesAddedOrChanged(object sender, SeriesAddedOrChangedArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (e.Index != m_index)
                    return;
                MatchCount = e?.NumberOfMatches ?? 0;
            });
        }

        public string SearchString
        {
            get => m_searchString;
            set
            {
                SetField(ref m_searchString, value);
                m_stringSubject.OnNext(value);
            }
        }

        public bool IsVisible
        {
            get => m_isVisible;
            set
            {
                SetField(ref m_isVisible, value);
                m_graphViewModel.ToggleSeriesVisibility(m_index);
            }
        }

        public int MatchCount
        {
            get => m_matchCount;
            private set => SetField(ref m_matchCount, value);
        }

        public bool IsReservedSearchString
        {
            get => m_isReservedSearchString;
            set => SetField(ref m_isReservedSearchString, value);
        }
    }

    public interface IFilterItemViewModel
    {
        string SearchString { get; set; }
        bool IsVisible { get; set; }
        int MatchCount { get; }
        bool IsReservedSearchString { get; set; }
    }
}