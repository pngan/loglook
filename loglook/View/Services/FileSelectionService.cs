using Microsoft.Win32;
using ViewModel;

namespace View.Services
{
    public class FileSelectionService : IFileSelectionService
    {
        public string GetFilePath(string defaultPath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "Document"; // Default file name
            openFileDialog.DefaultExt = ".txt"; // Default file extension
            openFileDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                if (openFileDialog.ShowDialog() == true)
                {
                    //Get the path of specified file
                    return openFileDialog.FileName;

                }
            

            return string.Empty;

            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.ShowDialog();
            //return openFileDialog.FileName;
        }
    }
}
