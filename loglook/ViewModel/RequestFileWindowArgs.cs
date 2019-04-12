using System;

namespace ViewModel
{
    public class RequestFileWindowArgs : EventArgs
    {
        public object WindowOwner { get; }
        public string FilePath { get; }

        public RequestFileWindowArgs(object windowOwner, string filePath)
        {
            WindowOwner = windowOwner;
            FilePath = filePath;
        }
    }
}