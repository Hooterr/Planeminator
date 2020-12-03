using Planeminator.Domain.Functions;
using System.Collections.Generic;

namespace Planeminator.Domain.Models
{
    /// <summary>
    /// Represent a single plane in the application
    /// </summary>
    public class Plane
    {
        /// <summary>
        /// Id of this plane
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Model of this plane
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Carrying capacity in KG
        /// </summary>
        public int CarryingCapacity { get; set; }

        /// <summary>
        /// Plane speed in kilometers per hour
        /// </summary>
        public double SpeedKmph { get; set; }

        /// <summary>
        /// The mileage of this plane
        /// </summary>
        public IMileageFunction Mileague { get; set; }
    }
}
