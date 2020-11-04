using Planeminator.Domain.Enumerations;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Abstract
{
    /// <summary>
    /// Represents a single flight in the application
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Id of this flight
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Originating airport
        /// </summary>
        public AirPort OriginatingAirport { get; set; }

        /// <summary>
        /// Destination ariport
        /// </summary>
        public AirPort DestinationAirport { get; set; }

        /// <summary>
        /// The plane that is flying
        /// </summary>
        public Plane Plane { get; set; }

        /// <summary>
        /// Date of the departure
        /// </summary>
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Planned arrival date
        /// </summary>
        public DateTime ArrivalDate { get; set; }

        /// <summary>
        /// Current status of the flight
        /// </summary>
        public FlightStatus Status { get; set; }
    }
}
