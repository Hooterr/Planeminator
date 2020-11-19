using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Planeminator.DataIO;
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

            DataIOServiceInstaller.Install(builder);

            Container = builder.Build();
            
            base.OnStartup(e);
        }
    }
}
