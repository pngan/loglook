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

        private readonly IMainModel m_model;
        private readonly IInteractionMediator m_interactionMediator;

        public MainViewModel(IMainModel model, IGraphViewModel graphViewModel, IInteractionMediator interactionMediator)
        {
            GraphViewModel = graphViewModel;
            m_model = model;
            m_interactionMediator = interactionMediator;
            OpenFileCommand = new RelayCommand(OpenFileCommandImpl);
        }

        private void OpenFileCommandImpl(object parameter)
        {
            m_interactionMediator.RequestFileWindow(this, parameter);
        }


        private string m_name;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                m_model.SetName(value);
            }
        }

        
        public void Start()
        {
            m_model.Start();
        }
    }
}