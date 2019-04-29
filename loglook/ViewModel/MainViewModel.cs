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


        public RelayCommand OpenFileCommand { get; private set; }

        //private readonly IFileModel m_model;
        private readonly IFileSelectionService m_fileSelectionService;
        private readonly IInteractionMediator m_interactionMediator;
        private readonly Func<Owned<IFileItemViewModel>> m_fileItemViewModelFactory;
        public List<Owned<IFileItemViewModel>> OwnedFileList { get; } = new List<Owned<IFileItemViewModel>>();
        public ObservableCollection<IFileItemViewModel> FileList { get; } = new ObservableCollection<IFileItemViewModel>();

        public MainViewModel(  IFileSelectionService fileSelectionService, 
            IInteractionMediator interactionMediator,  Func<Owned<IFileItemViewModel>> fileItemViewModelFactory)
        {
            m_fileSelectionService = fileSelectionService;
            m_interactionMediator = interactionMediator;
            m_fileItemViewModelFactory = fileItemViewModelFactory;
            OpenFileCommand = new RelayCommand(OpenFileCommandImpl);
        }

        private async void OpenFileCommandImpl(object windowOwner)
        {
            var path = m_fileSelectionService.GetFilePath("");
            var fileVm = m_fileItemViewModelFactory();
            if (await fileVm.Value.InitializeFileItemViewModel(path) == false)
            {

                return;
            }
            OwnedFileList.Add(fileVm);
            FileList.Add(fileVm.Value);
            var args = new RequestFileWindowArgs(windowOwner, path);

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