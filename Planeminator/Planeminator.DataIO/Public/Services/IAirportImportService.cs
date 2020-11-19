using Planeminator.DataIO.Public.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DataIO.Public.Services
{
    /// <summary>
    /// Handles importing operations for airports
    /// </summary>
    public interface IAirportImportService
    {
        /// <summary>
        /// Imports airports from JSON file
        /// </summary>
        /// <param name="absolutePath">The absolute path to the file</param>
        /// <returns>List of imported airports</returns>
        List<ImportedAirport> ImportAirportsFromJson(string absolutePath);
    }
}
