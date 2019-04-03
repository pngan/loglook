using System.Windows;
using Autofac;
using Model;
using Model.Services;
using ViewModel;

namespace loglook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private ILifetimeScope m_lifetime;

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            var container = builder.Build();

            StartApplication(container);
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<MainModel>().As<IMainModel>().InstancePerLifetimeScope();
            builder.RegisterType<MainViewModel>().As<IMainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<FileFilteredViewModel>().As<IFileFilteredViewModel>();
            builder.RegisterType<FileRawViewModel>().As<IFileRawViewModel>();
            builder.RegisterType<GraphViewModel>().As<IGraphViewModel>();
            builder.RegisterType<FilterListViewModel>().As<IFilterListViewModel>();
            builder.RegisterType<FilterItemViewModel>().As<IFilterItemViewModel>();
            builder.RegisterType<FileSelectionService>().As<IFileSelectionService>();

            builder.RegisterType<View.MainWindow>();
        }

        private void StartApplication(IContainer container)
        {
            m_lifetime = container.BeginLifetimeScope();
            var window = m_lifetime.Resolve<View.MainWindow>();
            window.Show();
        }
    }
}