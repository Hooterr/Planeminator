using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Functions
{
    /// <summary>
    /// Provides the information about mailage 
    /// </summary>
    public interface IMileageFunction
    {
        /// <summary>
        /// Calculates number of liters of fuel burned per kilometer
        /// </summary>
        /// <param name="loadKg">Mass of payload in kg</param>
        /// <returns>Mailage in liters per kilometer</returns>
        double CalculateMailage(double payloadMassKg);
    }
}
