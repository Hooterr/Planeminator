using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Planeminator.Algorithm.Public;
using Planeminator.Algorithm.Services;

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
            builder.RegisterType<SimulationBuilder>().As<ISimulationBuilder>();
            builder.RegisterType<ReportingService>().As<IReportingService>();

            builder.RegisterAutoMapper(typeof(AlgorithmServiceInstaller).Assembly);
        }
    }
}
