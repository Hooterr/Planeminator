using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public
{
    public class SimulationSettings
    {
        public List<Airport> Airports { get; set; }

        public List<Plane> Planes { get; set; }

        public int? Seed { get; set; }

        public double FuelPricePerLiter { get; set; }

        public int DurationInTimeUnits { get; set; }

        public PackageGenerationSettings PackageGeneration { get; set; }

    }
}
