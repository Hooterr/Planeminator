using Geolocation;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Planeminator.Algorithm.DataStructures
{
    internal class AirportDistanceMap : IAirportDistanceMap
    {
        private Dictionary<AlgorithmAirport, Dictionary<AlgorithmAirport, double>> _map;

        public double GetDistance(AlgorithmAirport from, AlgorithmAirport to)
        {
            return _map[from][to];
        }

        private AirportDistanceMap()
        { }
        
        public static IAirportDistanceMap FromAirports(List<AlgorithmAirport> airports)
        {
            if (airports == null || !airports.Any())
                throw new ArgumentException(nameof(airports));

            var len = airports.Count();
            var result = new AirportDistanceMap()
            {
                _map = new Dictionary<AlgorithmAirport, Dictionary<AlgorithmAirport, double>>(len),
            };
            
            foreach(var fromAirport in airports)
            {
                result._map[fromAirport] = new Dictionary<AlgorithmAirport, double>(len);

                foreach(var destinationAirport in airports)
                {
                    result._map[fromAirport][destinationAirport] = GeoCalculator.GetDistance(fromAirport.Coordinate, destinationAirport.Coordinate, distanceUnit: DistanceUnit.Kilometers);
                }
            }

            return result;
        }
    }
}
