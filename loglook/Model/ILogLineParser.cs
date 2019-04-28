using System;

namespace Model
{
    public interface ILogLineParser
    {
        DateTime? DateTimePart(string line);
        string LineContentPart(string line);
    }
}