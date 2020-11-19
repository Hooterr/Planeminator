using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DataIO.Import.Models
{
    /// <summary>
    /// Represents a geolocation point
    /// </summary>
    internal class GeolocationItem
    {
        /// <summary>
        /// Gets or sets latitude value
        /// </summary>
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets longitude value
        /// </summary>
        [JsonProperty("lng")]
        public double Longitude { get; set; }

    }
}
