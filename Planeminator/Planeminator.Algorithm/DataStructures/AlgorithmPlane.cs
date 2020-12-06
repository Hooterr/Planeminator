using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.DataStructures
{
    internal class AlgorithmPlane : Plane
    {
        public const int RouteLength = 7;

        public int InternalId { get; set; }

        public AlgorithmAirport CurrentAirport { get; set; }

        public List<AlgorithmPackage> Packages { get; set; }

        public List<AlgorithmAirport> Route { get; set; }

        public Plane AssociatedPlane { get; set; }

        public AlgorithmPlane()
        {
            CurrentAirport = new AlgorithmAirport();
            Packages = new List<AlgorithmPackage>();
            Route = new List<AlgorithmAirport>(RouteLength);
        }
    }
}
