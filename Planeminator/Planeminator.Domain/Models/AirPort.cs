using Geolocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Models
{
    /// <summary>
    /// Represents an airport in the application
    /// </summary>
    public class Airport
    {
        /// <summary>
        /// Id of this airport
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Coordinate Coordinate { get; set; }
    }
}
