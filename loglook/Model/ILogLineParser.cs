using System;
using System.Threading.Tasks;

namespace Model
{
    public interface ILogLineParser
    {
        Task<bool> IsApplicable(string path);
        DateTime? DateTimePart(string line);
        string LineContentPart(string line);
    }
}