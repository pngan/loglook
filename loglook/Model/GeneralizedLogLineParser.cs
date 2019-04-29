using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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
        private LogLineDescriptor m_logLineDescriptor = null;

        private readonly List<LogLineDescriptor> m_logLineDescriptors = new List<LogLineDescriptor>
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
                    m_logLineDescriptors = JsonConvert.DeserializeObject<List<LogLineDescriptor>>(configAsString);
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

        public async Task<bool> IsApplicable(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Log file does not exist.", path);
            }

            // Willing to look for timestamp in the first 
            using (var sr = File.OpenText(path))
            {
                int maxLinesToTest = 5;
                string s;
                while (((s = await sr.ReadLineAsync()) != null) && maxLinesToTest > 0 )
                {
                    if (DateTimePart(s) != null)
                        return true;
                    maxLinesToTest--;
                }
            }
            return false;
        }

        public DateTime? DateTimePart(string line)
        {
            if (m_logLineDescriptor == null)
            {
                // Evaluate the appropriate descriptor for the line
                foreach (var logLineDescriptor in m_logLineDescriptors)
                {
                    if (DateTimePartImpl(line, logLineDescriptor) != null)
                    {
                        m_logLineDescriptor = logLineDescriptor;
                        break;
                    }
                }
            }

            if (m_logLineDescriptor == null)
            {
                return null;
            }

            return DateTimePartImpl(line, m_logLineDescriptor);
        }


        private static DateTime? DateTimePartImpl(string line, LogLineDescriptor descriptor)
        {
            if (line.Length < descriptor.DatetimeIndexStart + descriptor.DatetimeLength)
                return null;
            var possibleTimeStamp = line.Substring(descriptor.DatetimeIndexStart, descriptor.DatetimeLength);

            return DateTime.TryParseExact(possibleTimeStamp, descriptor.DateTimeFormatString, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var t) ? t : (DateTime?)null;
        }



        public string LineContentPart(string line)
        {
            if (line.Length < m_logLineDescriptors[0].ContentIndexStart + 1)
                return null;
            return line.Substring(m_logLineDescriptors[0].ContentIndexStart);
        }
    }
}