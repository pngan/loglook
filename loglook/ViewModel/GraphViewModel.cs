using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class GraphViewModel : ViewModelBase, IGraphViewModel
    {
        private readonly IFileModel m_fileModel;
        private int? m_lineCount;

        public GraphViewModel(IFileModel fileModel)
        {
            m_fileModel = fileModel;
        }
        public string Name => "GraphViewModel";

        public int LineCount
        {
            get
            {
                if (m_lineCount != null)
                    return m_lineCount.Value;
                Task.Run(async () =>
                {
                    var lineCount = await GetLineCountAsync();
                    SetField(ref m_lineCount, lineCount);
                });
                return 0;
            }
        }


        private async Task<int> GetLineCountAsync()
        {
            return await m_fileModel.GetLineCountAsync();
        }
    }

    public interface IGraphViewModel
    {
        string Name { get; }
        int LineCount { get; }
    }
}
