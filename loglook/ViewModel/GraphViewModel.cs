using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Defaults;
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

            NewDataCommand = new RelayCommand(NewData);


            var r = new Random();
            ValuesA = new ChartValues<ObservablePoint>();
            ValuesB = new ChartValues<ObservablePoint>();
            ValuesC = new ChartValues<ObservablePoint>();

            for (var i = 0; i < 20; i++)
            {
                ValuesA.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                ValuesB.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
                ValuesC.Add(new ObservablePoint(r.NextDouble() * 10, r.NextDouble() * 10));
            }

        }

        private void NewData(object obj)
        {
            var r = new Random();
            for (var i = 0; i < 20; i++)
            {
                ValuesA[i].X = r.NextDouble() * 10;
                ValuesA[i].Y = r.NextDouble() * 10;
                ValuesB[i].X = r.NextDouble() * 10;
                ValuesB[i].Y = r.NextDouble() * 10;
                ValuesC[i].X = r.NextDouble() * 10;
                ValuesC[i].Y = r.NextDouble() * 10;
            }
        }

        public Dictionary<int, double> ChartValues { get; } = new Dictionary<int, double>();

        public ChartValues<ObservablePoint> ValuesA { get; set; }
        public ChartValues<ObservablePoint> ValuesB { get; set; }
        public ChartValues<ObservablePoint> ValuesC { get; set; }
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

        private async Task<int> GetLineCountAsync()
        {
            return await m_fileModel.GetLineCountAsync();
        }
    }

    public interface IGraphViewModel
    {
        string Name { get; }
        int LineCount { get; }
    }
}