using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Model
{
    //public class FileEntity
    //{
    //    public string FilePath { get; }
    //    public string[] FileContent { get; }

    //    public FileEntity(string filePath, string[] fileContent)
    //    {
    //        FilePath = filePath;
    //        FileContent = fileContent;
    //    }
    //}

    //public class MainModel : IMainModel
    //{
    //    private readonly IFileSelectionService m_fileSelectionService;
    //    public string LogContent { get; private set; }
    //    public List<FileEntity> Files { get; } = new List<FileEntity>();

    //    public MainModel(IFileSelectionService fileSelectionService)
    //    {
    //        m_fileSelectionService = fileSelectionService;
    //    }

    //    public void Start()
    //    {
            
    //    }

    //    public void LoadFile(string defaultPath)
    //    {
    //        var path = m_fileSelectionService.GetFilePath(defaultPath);
    //        var entity = new FileEntity(path, File.ReadAllLines(path));
    //        Files.Add(entity);
    //        OnFileAdded?.Invoke(this, entity);
    //    }

    //    public void SetName(string name)
    //    {
    //    }

    //    private void SetLogContent(string logContent)
    //    {
    //        LogContent = logContent;
    //    }

    //    public event EventHandler<FileEntity> OnFileAdded;
    //}
}