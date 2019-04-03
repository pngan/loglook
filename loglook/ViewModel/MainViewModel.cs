using Model;

namespace ViewModel
{
    public interface IMainViewModel
    {
        void Start();
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public IFileFilteredViewModel FileFilteredViewModel  { get;}
        public IFileRawViewModel FileRawViewModel { get;}
        public IFilterListViewModel FilterListViewModel { get;}
        public IGraphViewModel GraphViewModel { get;}

        private readonly IMainModel m_model;

        public MainViewModel(IMainModel model, IFileFilteredViewModel fileFilteredViewModel, IFileRawViewModel fileRawViewModel, IFilterListViewModel filterListViewModel, IGraphViewModel graphViewModel)
        {
            m_model = model;
            FileFilteredViewModel = fileFilteredViewModel;
            FileRawViewModel = fileRawViewModel;
            FilterListViewModel = filterListViewModel;
            GraphViewModel = graphViewModel;
            //m_model.OnGreetingChanged += OnGreetingChanged;
        }

        private void OnGreetingChanged(object sender, GreetingArgs e)
        {
            Greeting = e.Greeting;
        }

        private string m_name;
        private string m_greeting;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                m_model.SetName(value);
            }
        }

        public string Greeting
        {
            get { return m_greeting; }
            set
            {
                m_greeting = value;
                OnPropertyChanged("Greeting");
            }
        }
        
        public void Start()
        {
            m_model.Start();
        }
    }
}