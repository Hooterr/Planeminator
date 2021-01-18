using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.DataStructures.Genetic
{
    internal class Population
    {
        public List<PopulationElement> Elements { get; set; }
        public double ObjectiveFunctionValue { get; set; }

        public Population()
        {
            Elements = new List<PopulationElement>();
        }

    }

    internal class PopulationElement
    {
        public AlgorithmPlane Plane { get; set; }
        public List<AlgorithmAirport> Route { get; set; }

        public PopulationElement()
        {
            Route = new List<AlgorithmAirport>();
        }
    }
}
