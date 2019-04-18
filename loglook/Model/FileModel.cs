using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Model
{
    public class DatedDataSeries
    {
        public DatedDataSeries(List<DateModel> values, string title)
        {
            Values = values;
            Title = title;
        }
        public List<DateModel> Values { get; }
        public string Title { get; }

    }

    public class SeriesAddedOrChangedArgs : EventArgs
    {
        public DatedDataSeries DatedData { get; }
        public int NumberOfMatches { get; }

        public SeriesAddedOrChangedArgs(DatedDataSeries data, int numberOfMatches)
        {
            DatedData = data;
            NumberOfMatches = numberOfMatches;
        }
    }
    public class FileModel : IFileModel
    {

        public FileModel(string filePath)
        {
            FilePath = filePath;
        }

        public event EventHandler<SeriesAddedOrChangedArgs> OnSeriesAddedOrChanged;
        public string FilePath { get; }

        public async Task<int> GetLineCountAsync()
        {
            int lineCount = 0;
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Log file does not exist.", FilePath);
            }

            using (var sr = File.OpenText(FilePath))
            {
                string s;
                while (await sr.ReadLineAsync() != null)
                {
                    lineCount++;
                }
            }

            return lineCount;
        }

        public async Task AddOrChangeSearchString(string searchString)
        {
            if (!File.Exists(FilePath))
            {
                return;
            }

            int numberOfMatches = 0;
            var series = await Task.Run(async () =>
            {
                using (var sr = File.OpenText(FilePath))
                {
                    var values = new List<DateModel>();
                    string s;
                    while ((s = await sr.ReadLineAsync()) != null)
                    {
                        if (s.Length < 11)
                            continue;
                        var possibleTimeStamp = s.Substring(0, 11);
                        if (!DateTime.TryParse(possibleTimeStamp, out var t))
                            continue;

                        if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(s.Substring(11), searchString,
                                CompareOptions.IgnoreCase) < 0)
                            continue;   // no string match

                        var timeStamp = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
                        if (!values.Any())
                            values.Add(new DateModel(timeStamp));
                        var latest = values.Last();
                        if ((timeStamp - latest.DateTime) >= TimeSpan.FromSeconds(1))
                        {
                            latest = new DateModel(timeStamp);
                            values.Add(latest);
                        }
                        latest.IncrementCount();
                        numberOfMatches++;
                    }
                    return new DatedDataSeries(values, searchString);
                }
            });

            OnSeriesAddedOrChanged?.Invoke(this, new SeriesAddedOrChangedArgs(series, numberOfMatches));
        }
    }
}