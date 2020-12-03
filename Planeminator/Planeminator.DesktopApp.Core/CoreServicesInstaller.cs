using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DesktopApp.Core
{
    public static class CoreServicesInstaller
    {
        /// <summary>
        /// Installs the dependencies if needed
        /// </summary>
        /// <param name="builder">The builder to add the dependencies to</param>
        public static void Install(ContainerBuilder builder)
        {
            //builder.RegisterType<AirportImportService>().As<ISolvingService>();
        }
    }
}
