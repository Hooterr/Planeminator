using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Planeminator.Algorithm;
using Planeminator.DataIO;
using Planeminator.DesktopApp.Core;
using Planeminator.DesktopApp.Core.Services;
using Planeminator.DesktopApp.Core.ViewModels;
using Planeminator.DesktopApp.Services;
using Planeminator.DesktopApp.ViewModels;
using Planeminator.Domain.DI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Planeminator.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 

        protected override void OnStartup(StartupEventArgs e)
        {
            Framework.Construct(builder =>
            {
                builder.RegisterInstance(ApplicationViewModel.Instance).ExternallyOwned().SingleInstance();
                builder.RegisterType<MainPageViewModel>().AsSelf();
                builder.RegisterType<FileDialogService>().As<IFileDialogService>();
                builder.RegisterType<UIManager>().As<IUIManager>();

                DataIOServiceInstaller.Install(builder);
                AlgorithmServiceInstaller.Install(builder);
                CoreServicesInstaller.Install(builder);
            });

            base.OnStartup(e);
        }
    }
}
