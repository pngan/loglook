using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Model;

namespace ViewModel
{
    public class FileItemViewModel : ViewModelBase, IFileItemViewModel
    {
        private readonly IFileModel m_fileModel;

        public FileItemViewModel(IFileModel fileModel,
            Lazy<IFilterListViewModel> filterListViewModel, 
            Lazy<IGraphViewModel> graphViewModel)
        {
            m_fileModel = fileModel;
            m_filterListViewModel = filterListViewModel.Value;
            m_graphViewModel = graphViewModel.Value;
        }

        private readonly IFilterListViewModel m_filterListViewModel;
        private readonly IGraphViewModel m_graphViewModel;

        public IFilterListViewModel FilterListViewModel => m_filterListViewModel;

        public IGraphViewModel GraphViewModel => m_graphViewModel;
        public async Task<bool> InitializeFileItemViewModel(string path)
        {
            return await m_fileModel.InitializeFileModel(path);
        }

        public string Path => m_fileModel.FilePath;
    }

    public interface IFileItemViewModel
    {
        string Path { get; }
        IFilterListViewModel FilterListViewModel { get; }
        IGraphViewModel GraphViewModel { get; }

        Task<bool> InitializeFileItemViewModel(string path);
    }
}
