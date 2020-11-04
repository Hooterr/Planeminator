using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Enumerations
{
    /// <summary>
    /// Status of the flight as an enum
    /// </summary>
    public enum FlightStatus
    {
        /// <summary>
        /// The flight is awating for the departure
        /// </summary>
        AwatingDeparture = 0,

        /// <summary>
        /// The flight is in progres
        /// </summary>
        InProgress,

        /// <summary>
        /// The flight is finished
        /// </summary>
        Finished,
    }
}
