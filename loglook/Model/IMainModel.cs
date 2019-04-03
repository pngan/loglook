using System;

namespace Model
{
    public interface IMainModel
    {
        void Start();
        void SetName(string name);
        event EventHandler<FileEntity> OnFileAdded;

        string LogContent { get; }

        void LoadFile(string defaultPath);
    }
}