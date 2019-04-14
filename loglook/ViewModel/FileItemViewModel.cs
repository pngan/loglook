using System;
using Autofac.Features.OwnedInstances;
using Model;

namespace ViewModel
{
    public class FileItemViewModel : ViewModelBase, IFileItemViewModel
    {
        private readonly IFileModel m_fileModel;

        public FileItemViewModel(Func<string, IFileModel> fileModelFactory,
            Lazy<IFilterListViewModel> filterListViewModel, 
            Lazy<IGraphViewModel> graphViewModel,
            string path)
        {
            m_fileModel = fileModelFactory(path);
            //m_fileModel = fileModel();

            m_filterListViewModel = filterListViewModel.Value;
            m_graphViewModel = graphViewModel.Value;
            Path = path;
        }

        //private readonly Owned<IFileModel> m_fileModel;
        private readonly IFilterListViewModel m_filterListViewModel;
        private readonly IGraphViewModel m_graphViewModel;

        public IFilterListViewModel FilterListViewModel => m_filterListViewModel;

        public IGraphViewModel GraphViewModel => m_graphViewModel;


        public string Path { get; }
    }

    public interface IFileItemViewModel
    {
        string Path { get; }
        IFilterListViewModel FilterListViewModel { get; }
        IGraphViewModel GraphViewModel { get; }
    }
}
