using Geolocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Models
{
    /// <summary>
    /// Represents an airport in the application
    /// </summary>
    public class AirPort : IAirport
    {
        /// <summary>
        /// Id of the airport
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Packages currently stored at this airport
        /// </summary>
        public List<Package> Packages { get; set; }

        /// <summary>
        /// Planes available on this airport
        /// </summary>
        public List<Plane> AvailablePlanes { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
