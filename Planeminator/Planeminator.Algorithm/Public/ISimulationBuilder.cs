using Planeminator.Domain.Models;
using System.Collections.Generic;
using System.Numerics;
using Plane = Planeminator.Domain.Models.Plane;

namespace Planeminator.Algorithm.Public
{
    public interface ISimulationBuilder
    {
        ISimulationBuilder WithPlanes(List<Plane> planes);

        ISimulationBuilder WithAirports(List<Airport> airports);

        ISimulationBuilder WithSeed(int seed);

        Simulation Build();
    }
}
