using Geolocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Models
{
    public interface IAirport
    {
        /// <summary>
        /// Coordinates of this airport
        /// </summary>
        public Coordinate Coordinate { get; }

        /// <summary>
        /// Packages currently stored at this airport
        /// </summary>
        public List<Package> Packages { get; }

        /// <summary>
        /// Planes available on this airport
        /// </summary>
        public List<Plane> AvailablePlanes { get; }
    }
}
