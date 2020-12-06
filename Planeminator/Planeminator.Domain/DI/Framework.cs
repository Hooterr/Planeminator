using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.DI
{
    public static class Framework
    {
        public static IContainer Container { get; private set; }

        public static void Construct(Action<ContainerBuilder> configure)
        {
            var builder = new ContainerBuilder();

            configure(builder);

            Container = builder.Build();
        }
    }
}
