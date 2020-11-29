using Geolocation;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Planeminator.Algorithm.DataStructures
{
    internal class AirportDistanceMap : IAirportDistanceMap
    {
        private Dictionary<IAirport, Dictionary<IAirport, double>> _map;

        public double GetDistance(IAirport from, IAirport to)
        {
            return _map[from][to];
        }

        private AirportDistanceMap()
        { }
        
        public static IAirportDistanceMap FromAirports(List<IAirport> airports)
        {
            if (airports == null || !airports.Any())
                throw new ArgumentException(nameof(airports));

            var len = airports.Count();
            var result = new AirportDistanceMap()
            {
                _map = new Dictionary<IAirport, Dictionary<IAirport, double>>(len),
            };
            
            foreach(var fromAirport in airports)
            {
                result._map[fromAirport] = new Dictionary<IAirport, double>(len);

                foreach(var destinationAirport in airports)
                {
                    result._map[fromAirport][destinationAirport] = GeoCalculator.GetDistance(fromAirport.Coordinate, destinationAirport.Coordinate, distanceUnit: DistanceUnit.Kilometers);
                }
            }

            return result;
        }
    }
}
