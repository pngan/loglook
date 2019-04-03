using System.Windows;
using Autofac;
using Model;
using ViewModel;

namespace loglook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            var container = builder.Build();

            StartApplication(container);
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<MainModel>().As<IMainModel>();
            builder.RegisterType<MainViewModel>().As<IMainViewModel>();
            builder.RegisterType<FileFilteredViewModel>().As<IFileFilteredViewModel>();
            builder.RegisterType<FileRawViewModel>().As<IFileRawViewModel>();
            builder.RegisterType<GraphViewModel>().As<IGraphViewModel>();
            builder.RegisterType<FilterListViewModel>().As<IFilterListViewModel>();
            builder.RegisterType<FilterItemViewModel>().As<IFilterItemViewModel>();

            builder.RegisterType<View.MainWindow>();
        }

        private static void StartApplication(IContainer container)
        {
            var window = container.Resolve<View.MainWindow>();
            window.Show();
        }
    }
}