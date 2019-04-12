using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autofac.Features.OwnedInstances;
using Model;

namespace ViewModel
{
    public interface IMainViewModel
    {
        void Start();
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {

        public IGraphViewModel GraphViewModel { get; }

        public RelayCommand OpenFileCommand { get; private set; }

        //private readonly IMainModel m_model;
        private readonly IFileSelectionService m_fileSelectionService;
        private readonly IInteractionMediator m_interactionMediator;
        private readonly Func<string, Owned<IFileItemViewModel>> m_fileItemViewModelFactory;
        public List<Owned<IFileItemViewModel>> OwnedFileList { get; } = new List<Owned<IFileItemViewModel>>();
        public ObservableCollection<IFileItemViewModel> FileList { get; } = new ObservableCollection<IFileItemViewModel>();

        public MainViewModel( IGraphViewModel graphViewModel, IFileSelectionService fileSelectionService, 
            IInteractionMediator interactionMediator, Func<string, Owned<IFileItemViewModel>> fileItemViewModelFactory)
        {
            GraphViewModel = graphViewModel;
            m_fileSelectionService = fileSelectionService;
            m_interactionMediator = interactionMediator;
            m_fileItemViewModelFactory = fileItemViewModelFactory;
            OpenFileCommand = new RelayCommand(OpenFileCommandImpl);
        }

        private void OpenFileCommandImpl(object windowOwner)
        {
            var path = m_fileSelectionService.GetFilePath("");
            var fileVm = m_fileItemViewModelFactory(path);
            OwnedFileList.Add(fileVm);
            FileList.Add(fileVm.Value);





            var args = new RequestFileWindowArgs(windowOwner, path);
            //m_interactionMediator.RequestFileWindow(this, args);
        }


        private string m_name;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                //m_model.SetName(value);
            }
        }

        
        public void Start()
        {
            //m_model.Start();
        }
    }
}