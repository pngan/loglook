﻿using System.Windows;
using Autofac;
using Model;
using View;
using View.Services;
using ViewModel;
using IFileSelectionService = ViewModel.IFileSelectionService;

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
            builder.RegisterType<FileModel>().As<IFileModel>().InstancePerOwned<IFileItemViewModel>();
            builder.RegisterType<MainViewModel>().As<IMainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemViewModel>().As<IFileItemViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<FileFilteredViewModel>().As<IFileFilteredViewModel>();
            builder.RegisterType<FileRawViewModel>().As<IFileRawViewModel>();
            builder.RegisterType<GraphViewModel>().As<IGraphViewModel>().InstancePerOwned<IFileItemViewModel>();
            builder.RegisterType<FilterListViewModel>().As<IFilterListViewModel>();
            builder.RegisterType<FilterItemViewModel>().As<IFilterItemViewModel>();
            builder.RegisterType<FileSelectionService>().As<IFileSelectionService>();
            //builder.RegisterType<FileWindowService>().SingleInstance().AutoActivate();
            builder.RegisterType<InteractionMediator>().As<IInteractionMediator>().SingleInstance();
            builder.RegisterType<FileWindowViewModel>().As<IFileWindowViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<GeneralizedLogLineParser>().As<ILogLineParser>();
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