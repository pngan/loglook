using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Model;

namespace ViewModel
{
    public class FilterItemViewModel : ViewModelBase, IFilterItemViewModel
    {
        private readonly IFileModel m_fileModel;
        private string m_searchString;
        private bool m_isVisible = true;
        private readonly Subject<string> m_stringSubject = new Subject<string>();
        private int m_matchCount;

        public FilterItemViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
            MatchCount = 0;
            IObserver<string> obs;
            m_stringSubject.AsObservable()
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {
                    m_fileModel.AddOrChangeSearchString(x);
                });
            m_fileModel.OnSeriesAddedOrChanged += FileModelOnOnSeriesAddedOrChanged;

        }

        private void FileModelOnOnSeriesAddedOrChanged(object sender, SeriesAddedOrChangedArgs e)
        {
            MatchCount = e?.NumberOfMatches ?? 0;
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
            set => SetField(ref m_isVisible, value);
        }

        public int MatchCount
        {
            get => m_matchCount;
            private set => SetField(ref m_matchCount, value);
        }
    }

    public interface IFilterItemViewModel
    {
        string SearchString { get; set; }
        bool IsVisible { get; set; }
        int MatchCount { get; }
    }
}