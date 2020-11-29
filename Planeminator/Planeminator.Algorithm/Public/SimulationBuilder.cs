using Planeminator.Algorithm.Services;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public
{
    public class SimulationBuilder
    {
        private List<Plane> planes;
        private List<IAirport> airports;

        public SimulationBuilder WithPlanes(List<Plane> planes)
        {
            this.planes = planes;
            return this;
        }

        public SimulationBuilder WithAirports(List<IAirport> airports)
        {
            this.airports = airports;
            return this;
        }

        public Simulation Build()
        {
            var result = new SimulationImplementation(airports);

            return result;
        }
    }
}
