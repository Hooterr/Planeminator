using Planeminator.Domain.Functions;
using System;
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
        /// The mileage of this plane
        /// </summary>
        public IMileageFunction Mileague { get; set; }
    }
}
