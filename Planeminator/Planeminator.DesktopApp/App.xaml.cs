using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Planeminator.Algorithm;
using Planeminator.DataIO;
using Planeminator.DesktopApp.Core;
using Planeminator.DesktopApp.Core.ViewModels;
using Planeminator.DesktopApp.ViewModels;
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
        public static IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(ApplicationViewModel.Instance).ExternallyOwned().SingleInstance();
            builder.RegisterType<MainPageViewModel>().AsSelf();

            DataIOServiceInstaller.Install(builder);
            AlgorithmServiceInstaller.Install(builder);
            CoreServicesInstaller.Install(builder);

            Container = builder.Build();
            
            base.OnStartup(e);
        }
    }
}
