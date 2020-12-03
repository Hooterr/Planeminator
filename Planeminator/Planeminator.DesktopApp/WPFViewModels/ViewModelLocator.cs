using Autofac;
using Planeminator.DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeminator.DesktopApp.WPFViewModels
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        public ApplicationViewModel ApplicationViewModel => App.Container.BeginLifetimeScope().Resolve<ApplicationViewModel>();
    }
}
