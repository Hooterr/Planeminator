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

        /// <summary>
        /// Gets or sets the name of the city this airport is located in
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the name of the city this airpot is located in
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets IATA code of this airport
        /// </summary>
        public string IATACode { get; set; }
    }
}
