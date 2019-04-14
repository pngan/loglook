using System.IO;
using System.Threading.Tasks;

namespace Model
{
    public class FileModel : IFileModel
    {

        public FileModel(string filePath)
        {
            FilePath = filePath;
        }

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
    }
}