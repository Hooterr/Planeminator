using Autofac;
using Planeminator.Domain;
using Planeminator.Domain.Models;
using Planeminator.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm
{
    public static class AlgorithmServiceInstaller
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
