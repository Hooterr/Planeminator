using Geolocation;
using Planeminator.Domain.Models;
using System.Collections.Generic;

namespace Planeminator.Algorithm.DataStructures
{
    internal class AlgorithmAirport : Airport
    {
        public int InternalId { get; set; }

        public List<AlgorithmPackage> Packages { get; set; }

        public List<AlgorithmPlane> AvailablePlanes { get; set; }


        public AlgorithmAirport()
        {
            Packages = new List<AlgorithmPackage>();
            AvailablePlanes = new List<AlgorithmPlane>();
        }
    }
}
