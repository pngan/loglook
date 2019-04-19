using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Model;

namespace ViewModel
{
    public class GraphViewModel : ViewModelBase, IGraphViewModel
    {
        private readonly IFileModel m_fileModel;
        private int? m_lineCount;

        public GraphViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
            m_fileModel.OnSeriesAddedOrChanged += FileModelOnOnSeriesAddedOrChanged;

            InspectDataCommand = new RelayCommand(InspectData);

            Formatter = value => new DateTime((long) (value * TimeSpan.FromHours(1).Ticks)).ToString("t");

            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double) dayModel.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Value);

            SeriesCollection = new SeriesCollection(dayConfig);
            CreateSeries("");
        }

        public RelayCommand InspectDataCommand { get; }

        public Func<double, string> Formatter { get; set; }
        public void ToggleSeriesVisibility(int index)
        {
            if (index >= SeriesCollection.Count)
                return;
            var series = ((ScatterSeries)SeriesCollection[index]);
            series.Visibility = series.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        public int LineCount
        {
            get
            {
                if (m_lineCount != null)
                    return m_lineCount.Value;
                Task.Run(async () =>
                {
                    var lineCount = await GetLineCountAsync();
                    SetField(ref m_lineCount, lineCount);
                });
                return 0;
            }
        }

        public SeriesCollection SeriesCollection { get; }

        private void CreateSeries(string legend)
        {
            var series = new ScatterSeries();
            series.Title = legend;
            var v = new ChartValues<DateModel>();
            series.Values = v;
            SeriesCollection.Add(series);
        }

        private void FileModelOnOnSeriesAddedOrChanged(object sender, SeriesAddedOrChangedArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var index = e.Index;
                if (SeriesCollection.Count <= index)
                    CreateSeries(e.DatedData.Title);
                var values = SeriesCollection[index].Values;
                values.Clear();
                values.AddRange(e?.DatedData?.Values);
                ((ScatterSeries) SeriesCollection[index]).Title = e.DatedData.Title;
            });
        }

        private void InspectData(object obj)
        {
            var pt = (ChartPoint) obj;
            var dataPt = pt.Instance as DateModel;
            if (dataPt != null)
            {
                Process.Start("notepad++", $"-n{dataPt.LineNumber} {m_fileModel.FilePath}");
            }
        }

        private async Task<int> GetLineCountAsync()
        {
            return await m_fileModel.GetLineCountAsync();
        }
    }

    public interface IGraphViewModel
    {
        int LineCount { get; }

        SeriesCollection SeriesCollection { get; }

        Func<double, string> Formatter { get; }
        void ToggleSeriesVisibility(int index);
    }
}