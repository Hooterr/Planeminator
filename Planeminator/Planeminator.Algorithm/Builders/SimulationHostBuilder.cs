using Planeminator.Algorithm.Services;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Builders
{
    internal class SimulationHostBuilder : ISimulationHostBuilder
    {
        private List<IAirport> airports;
        private List<Plane> planes;

        public ISimulationHostBuilder AddAirports(List<IAirport> airports)
        {
            this.airports = airports;
            return this;
        }

        public ISimulationHostBuilder AddPlanes(List<Plane> planes)
        {
            this.planes = planes;
            return this;
        }

        public ISimulationHostBuilder AddRecorder(Type recorderType)
        {
            throw new NotImplementedException();
        }

        public ISimulationHost Build()
        {
            throw new NotImplementedException();
        }
    }
}
