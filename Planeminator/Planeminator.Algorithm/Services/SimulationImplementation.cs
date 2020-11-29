using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public;
using Planeminator.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Planeminator.Algorithm.Services
{
    internal class SimulationImplementation : Simulation
    {
        private readonly IAirportDistanceMap DistanceMap;

        public SimulationImplementation(List<IAirport> airports)
        {
            DistanceMap = AirportDistanceMap.FromAirports(airports);
        }

        public override async Task<bool> Start()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(500);
            }

            return true;
        }
    }
}
