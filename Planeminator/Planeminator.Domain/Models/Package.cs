using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Models
{
    /// <summary>
    /// Represents a package in the application
    /// </summary>
    [Obsolete]
    public class Package
    {
        /// <summary>
        /// Deadlie to deliever the package to the destination airport
        /// </summary>
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// Income from delievering the package
        /// </summary>
        public double Income { get; set; }
    }
}
