using Planeminator.Algorithm.Services;
using System;

namespace Planeminator.Algorithm.Public
{
    public delegate void ProgressHandler(int round, int interation);

    public class SimulationBuilder
    {
        public SimulationSettings Settings { get; set; }

        public ProgressHandler ProgressHandler { get; set; }

        private SimulationBuilder()
        {
        }

        public Simulation Build()
        {
            if (Settings == null)
                throw new InvalidOperationException("Provide settings for the simulation first.");

            return new SimulationImplementation(Settings, ProgressHandler);
        }

        public static SimulationBuilder New()
        {
            return new SimulationBuilder();
        }
    }
}
