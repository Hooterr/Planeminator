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
        private readonly IMapper mapper;

        public SimulationBuilder(IMapper _mapper)
        {
            mapper = _mapper;
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
            var algAirports = mapper.Map<List<AlgorithmAirport>>(airports);
            var algPlanes = mapper.Map<List<AlgorithmPlane>>(planes);

            var idCurrent = 0;
            foreach (var airport in algAirports)
            {
                airport.InternalId = idCurrent++;
                airport.Packages = new List<AlgorithmPackage>();
            }

            idCurrent = 0;
            foreach (var plane in algPlanes)
            {
                plane.InternalId = idCurrent++;
                plane.Packages = new List<AlgorithmPackage>();
            }

            var sim = new SimulationImplementation(algAirports, algPlanes);
            return sim;
        }
    }
}
