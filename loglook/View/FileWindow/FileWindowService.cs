using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac.Features.OwnedInstances;
using ViewModel;

namespace View
{
    public class FileWindowService : IFileWindowService
    {
        private readonly Func<Owned<IFileWindowViewModel>> m_fileWindowViewModelFactory;

        private readonly List<Owned<IFileWindowViewModel>> m_fileWindowViewModelList = new List<Owned<IFileWindowViewModel>>();
        public FileWindowService(IInteractionMediator interactionMediator, Func<Owned<IFileWindowViewModel>> fileWindowViewModelFactory)
        {
            m_fileWindowViewModelFactory = fileWindowViewModelFactory;
            interactionMediator.OnRequestFileWindow += ShowWindow;
        }

        private void ShowWindow(object sender, ObjectEventArgs e)
        {
            var fileWindowViewModel = m_fileWindowViewModelFactory();
            m_fileWindowViewModelList.Add(fileWindowViewModel);
            var win = new FileWindow {DataContext = fileWindowViewModel.Value, Owner = (Window) e.Value};
            win.Show();
        }
    }

    public interface IFileWindowService
    {
    }
}
