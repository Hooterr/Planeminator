using Planeminator.Domain.Interfaces;
using Planeminator.Domain.Models;
using System;

namespace Planeminator.Algorithm.DataStructures
{
    internal class AlgorithmPackage : IPackage<AlgorithmAirport>
    {
        public int Id { get; set; }

        public int DeadlineInTimeUnits { get; set; }

        public double Income { get; set; }
        
        public double MassKg { get; set; }

        public AlgorithmAirport Origin { get; set; }
        
        public AlgorithmAirport Destination { get; set; }
    }
}
