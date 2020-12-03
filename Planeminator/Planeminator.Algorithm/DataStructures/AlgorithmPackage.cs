using Planeminator.Domain.Models;
using System;

namespace Planeminator.Algorithm.DataStructures
{
    internal class AlgorithmPackage
    {
        public int InternalId { get; set; }

        public int DeadlineInTimeUnits { get; set; }

        public double Income { get; set; }
        
        public double MassKg { get; set; }

        public AlgorithmAirport Origin { get; set; }
        
        public AlgorithmAirport Destination { get; set; }

        public double GetPenalty(int currentTimeUnitNr)
        {
            return Math.Max(0, (currentTimeUnitNr - DeadlineInTimeUnits) * 1);
        }
    }
}
