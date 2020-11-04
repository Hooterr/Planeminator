using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Models
{
    /// <summary>
    /// Represents a package in the application
    /// </summary>
    public class Package
    {
        /// <summary>
        /// Id of the package
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Deadlie to deliever the package to the destination airport
        /// </summary>
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// Income from delievering the package
        /// </summary>
        public double Income { get; set; }

        /// <summary>
        /// The original airport the packages comes from 
        /// </summary>
        public AirPort OriginatingAirport { get; set; }

        /// <summary>
        /// The destination airport where the packaged should be transpored before <see cref="DeadLine"/>
        /// </summary>
        public AirPort DestinationAirport { get; set; }
    }
}
