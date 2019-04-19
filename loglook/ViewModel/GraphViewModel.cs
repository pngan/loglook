using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Model;

namespace ViewModel
{
    public class GraphViewModel : ViewModelBase, IGraphViewModel
    {
        private readonly IFileModel m_fileModel;
        private int? m_lineCount;
        private double m_xPointer;
        private double m_yPointer;

        public GraphViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
            m_fileModel.OnSeriesAddedOrChanged += FileModelOnOnSeriesAddedOrChanged;

            InspectDataCommand = new RelayCommand(InspectData);
            MouseMoveCommand = new RelayCommand(DoMouseMove);

            Formatter = value => new DateTime((long) (value * TimeSpan.FromHours(1).Ticks)).ToString("t");

            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double) dayModel.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Value);

            SeriesCollection = new SeriesCollection(dayConfig);
            CreateSeries("");
        }

        private void DoMouseMove(object obj)
        {
            var chart = (LiveCharts.Wpf.CartesianChart)obj;
            Point mouseCoordinate = Mouse.GetPosition(chart);

            ////lets get where the mouse is at our chart
            //var mouseCoordinate = e.GetPosition(chart);

            //now that we know where the mouse is, lets use
            //ConverToChartValues extension
            //it takes a point in pixes and scales it to our chart current scale/values
            var p = chart.ConvertToChartValues(mouseCoordinate);

            //in the Y section, lets use the raw value
            YPointer = p.Y;

            //for X in this case we will only highlight the closest point.
            //lets use the already defined ClosestPointTo extension
            //it will return the closest ChartPoint to a value according to an axis.
            //here we get the closest point to p.X according to the X axis
            var series = chart.Series[0];
            var closetsPoint = series.ClosestPointTo(p.X, AxisOrientation.X);

            XPointer = closetsPoint.X;

        }

        public RelayCommand InspectDataCommand { get; }
        public RelayCommand MouseMoveCommand { get; }

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

        public double XPointer
        {
            get => m_xPointer;
            set => SetField(ref m_xPointer, value);
        }

        public double YPointer
        {
            get => m_yPointer;
            set => SetField(ref m_yPointer, value);
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

        double XPointer { get; }
        double YPointer { get; }
    }
}