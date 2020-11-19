using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Planeminator.DataIO.Public.Services;
using Planeminator.DataIO.Services;

namespace Planeminator.DataIO
{
    /// <summary>
    /// Handles dependency installation
    /// </summary>
    public static class DataIOServiceInstaller
    {
        /// <summary>
        /// Installs the dependencies if needed
        /// </summary>
        /// <param name="builder">The builder to add the dependencies to</param>
        public static void Install(ContainerBuilder builder)
        {
            builder.RegisterType<AirportImportService>().As<IAirportImportService>();

            builder.RegisterAutoMapper(typeof(DataIOServiceInstaller).Assembly);
        }
    }
}
