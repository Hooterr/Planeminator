using Geolocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DataIO.Public.Models
{
    /// <summary>
    /// Imported airport as publicly available class
    /// </summary>
    public class ImportedAirport
    {
        /// <summary>
        /// Gets or sets the name of this airport
        /// </summary>
        public string Name { get; set; }

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

        /// <summary>
        /// Gets or sets the coordinate of this airport
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// Gets or sets the number of links this airport has
        /// </summary>
        public int LinksCount { get; set; }

        /// <summary>
        /// Gets or sets ID of this object
        /// </summary>
        public int ObjectId { get; set; }
    }
}
