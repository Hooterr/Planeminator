using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DataIO.Import.Models
{
    /// <summary>
    /// Model of a single airport in an imported JSON file
    /// </summary>
    internal class AirportListItem
    {
        /// <summary>
        /// Gets or sets the name of this airport
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the city this airport is located in
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the name of the city this airpot is located in
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets IATA code of this airport
        /// </summary>
        [JsonProperty("iata_code")]
        public string IATACode { get; set; }

        /// <summary>
        /// Gets or sets the geolocation of this airport
        /// </summary>
        [JsonProperty("_geoloc")]
        public GeolocationItem Geolocation { get; set; }
        
        /// <summary>
        /// Gets or sets the number of links this airport has
        /// </summary>
        [JsonProperty("links_count")]
        public int LinksCount { get; set; }

        /// <summary>
        /// Gets or sets ID of this object
        /// </summary>
        [JsonProperty("objectID")]
        public int ObjectId { get; set; }
    }
}
