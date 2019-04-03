using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public class FileSelectionService : IFileSelectionService
    {
        public string GetFilePath(string defaultPath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            return openFileDialog.FileName;
        }
    }

    public interface IFileSelectionService
    {
        string GetFilePath(string defaultPath);
    }
}
