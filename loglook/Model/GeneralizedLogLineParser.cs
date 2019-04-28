using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Model
{

    public class LogLineDescriptor
    {
        public string FileType { get; set; }
        public string FileTypeDescription { get; set; }
        public string LogLineExample { get; set; }
        public string DateTimeFormatString { get; set; }
        public int ContentIndexStart { get; set; }
        public int DatetimeIndexStart { get; set; }
        public int DatetimeLength { get; set; }
    }

    public class GeneralizedLogLineParser : ILogLineParser
    {
        private List<LogLineDescriptor> m_logLineDescriptor = new List<LogLineDescriptor>
        {
            new LogLineDescriptor
            {
                ContentIndexStart = 11,
                DatetimeIndexStart = 0,
                DatetimeLength = 11
            }
        };

        public GeneralizedLogLineParser()
        {
            var parserParamFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LogFileDescriptor.json";
            if (File.Exists(parserParamFile))
            {
                try
                {
                    var configAsString = File.ReadAllText(parserParamFile);
                    m_logLineDescriptor = JsonConvert.DeserializeObject<List<LogLineDescriptor>>(configAsString);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"No permission to read log file descriptor file: [{parserParamFile}].");
                }

            }
            else
            {
                Console.WriteLine($"Missing log file descriptor file: [{parserParamFile}]. Re-run loglook installer.");
            }
        }

        public DateTime? DateTimePart(string line)
        {
            if (line.Length < 11)
                return null;
            var possibleTimeStamp = line.Substring(m_logLineDescriptor[0].DatetimeIndexStart, m_logLineDescriptor[0].DatetimeLength);

            return DateTime.TryParseExact(possibleTimeStamp, m_logLineDescriptor[0].DateTimeFormatString, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var t) ? t : (DateTime?) null;
        }

        public string LineContentPart(string line)
        {
            if (line.Length < m_logLineDescriptor[0].ContentIndexStart + 1)
                return null;
            return line.Substring(m_logLineDescriptor[0].ContentIndexStart);
        }
    }
}