using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using Model;

namespace ViewModel
{
    public class GraphViewModel : ViewModelBase, IGraphViewModel
    {
        private readonly IFileModel m_fileModel;
        private int? m_lineCount;
        private readonly SeriesCollection m_seriesCollection;

        public Func<double, string> Formatter { get; set; }

        public GraphViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
            m_fileModel.OnSeriesAddedOrChanged += FileModelOnOnSeriesAddedOrChanged;

            NewDataCommand = new RelayCommand(NewData);

            Formatter = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("t");


            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Value);

            m_seriesCollection = new SeriesCollection(dayConfig);
            var series = new ScatterSeries();
            series.Title = m_fileModel.FilePath;
            var v = new ChartValues<DateModel>();
            var r = new Random();
            v.Add(new DateModel(DateTime.Now, r.Next(0, 100) * 10));
            v.Add(new DateModel(DateTime.Now - TimeSpan.FromHours(0.5), r.Next(0, 100) * 10));
            v.Add(new DateModel(DateTime.Now - TimeSpan.FromHours(1), r.Next(0, 100) * 10));
            series.Values = v;
            m_seriesCollection.Add(series);
        }

        private void FileModelOnOnSeriesAddedOrChanged(object sender, SeriesAddedOrChangedArgs e)
        {
            var values = m_seriesCollection.First().Values;
            values.Clear();
            values.AddRange(e?.DatedData?.Values);
        }

        private void NewData(object obj)
        {
            var r = new Random();
            var values = m_seriesCollection.First().Values;
            values.Clear();
            values.Add(new DateModel(DateTime.Now, r.Next(0, 100) * 10));
            values.Add(new DateModel(DateTime.Now - TimeSpan.FromHours(0.5), r.Next(0, 100) * 10));
            values.Add(new DateModel(DateTime.Now - TimeSpan.FromHours(1), r.Next(0, 100) * 10));
        }
        public string Name => "GraphViewModel";

        public RelayCommand NewDataCommand { get; }

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

        public SeriesCollection SeriesCollection => m_seriesCollection;

        private async Task<int> GetLineCountAsync()
        {
            return await m_fileModel.GetLineCountAsync();
        }
    }

    public interface IGraphViewModel
    {
        string Name { get; }
        int LineCount { get; }

        SeriesCollection SeriesCollection { get; }

        Func<double, string> Formatter { get; }
    }
}