using AutoMapper;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;

namespace Planeminator.Algorithm.Services
{
    internal class SimulationBuilder : ISimulationBuilder
    {
        private List<Plane> planes;
        private List<Airport> airports;
        private int? seed;

        public SimulationBuilder()
        {
        }

        public ISimulationBuilder WithPlanes(List<Plane> planes)
        {
            this.planes = planes;
            return this;
        }

        public ISimulationBuilder WithAirports(List<Airport> airports)
        {
            this.airports = airports;
            return this;
        }

        public ISimulationBuilder WithSeed(int seed)
        {
            this.seed = seed;
            return this;
        }

        public Simulation Build()
        {
            SimulationImplementation sim = null;

            if (seed.HasValue)
                sim = new SimulationImplementation(airports, planes, seed.Value);
            else
                sim = new SimulationImplementation(airports, planes);

            return sim;
        }
    }
}
