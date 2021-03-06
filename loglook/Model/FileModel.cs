﻿using System;
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
        
        public int Index { get; }

        public SeriesAddedOrChangedArgs(DatedDataSeries data, int numberOfMatches, int index)
        {
            DatedData = data;
            NumberOfMatches = numberOfMatches;
            Index = index;
        }
    }

    public class FileModel : IFileModel
    {
        private readonly IEnumerable<ILogLineParser> m_logLineParsers;
        private ILogLineParser m_logLineParser;

        public FileModel(IEnumerable<ILogLineParser> logLineParsers)
        {
            m_logLineParsers = logLineParsers;
        }

        public event EventHandler<SeriesAddedOrChangedArgs> OnSeriesAddedOrChanged;
        public string FilePath { get; private set; }

        public async Task<bool> InitializeFileModel(string filePath)
        {
            FilePath = filePath;

            // Find the correct log line parser
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Log file does not exist.", FilePath);
            }

            foreach (var logLineParser in m_logLineParsers)
            {
                if (await logLineParser.IsApplicable(filePath))
                {
                    m_logLineParser = logLineParser;
                    return true;
                }
            }

            return false;
        }

        public async Task<int> GetLineCountAsync()
        {
            int lineCount = 0;
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Log file does not exist.", FilePath);
            }

            using (var sr = File.OpenText(FilePath))
            {
                while (await sr.ReadLineAsync() != null)
                {
                    lineCount++;
                }
            }

            return lineCount;
        }

        public async Task AddOrChangeSearchString(int index, string searchString)
        {
            if (!File.Exists(FilePath) || string.IsNullOrWhiteSpace(searchString) || m_logLineParser == null)
            {
                return;
            }

            int secondsPerBin = 1;
            int numDataPoints = 0;
            int totalMatches = 0;
            var series = await Task.Run(async () =>
            {
                // The number of plotted data points is limited to 400, to optimize plotting.
                // Do this by repeated binning the data, with increasing bin sizes until
                // the number of data points falls below the threshold of 400.
                const int maxNumDataPoints = 400;
                var values = new List<DateModel>();
                do
                {
                    totalMatches = 0;
                    numDataPoints = 0;
                    int lineNumber = 0;
                    values.Clear();
                    using (var sr = File.OpenText(FilePath))
                    {
                        string s;
                        while ((s = await sr.ReadLineAsync()) != null)
                        {
                            lineNumber++;

                            var t = m_logLineParsers.First().DateTimePart(s);
                            if (t == null)
                                continue;

                            var line = m_logLineParsers.First().LineContentPart(s);

                            if (!searchString.Equals("*All*", StringComparison.Ordinal))
                                if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(line, searchString,
                                        CompareOptions.IgnoreCase) < 0)
                                    continue; // no string match

                            var timeStamp = new DateTime(t.Value.Year, t.Value.Month, t.Value.Day, t.Value.Hour, t.Value.Minute, t.Value.Second);
                            if (!values.Any())
                            {
                                values.Add(new DateModel(timeStamp));
                                numDataPoints++;
                            }

                            DateModel latestPoint = values.Last();
                            if ((timeStamp - latestPoint.DateTime) >= TimeSpan.FromSeconds(secondsPerBin))
                            {
                                latestPoint = new DateModel(timeStamp);
                                values.Add(latestPoint);
                                numDataPoints++;
                            }

                            latestPoint.IncrementCount();
                            latestPoint.LineNumber = lineNumber;
                            totalMatches++;
                        }
                    }

                    // Increase the bin size by a ratio of the number times the datapoints compared to 400.
                    // Clamp the multiplier between 2 and 10 so that the binning is not too severe.
                    var binMultiplier = Math.Max(2, Math.Min(numDataPoints / maxNumDataPoints, 10));
                    secondsPerBin *= binMultiplier;

                } while (numDataPoints > maxNumDataPoints); // Iterate if too many datapoints, but next time with larger bin size

                return new DatedDataSeries(values, searchString);
            });

            OnSeriesAddedOrChanged?.Invoke(this, new SeriesAddedOrChangedArgs(series, totalMatches, index));
        }
    }
}