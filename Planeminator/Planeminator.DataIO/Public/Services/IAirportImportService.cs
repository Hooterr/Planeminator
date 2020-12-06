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

        /// <summary>
        /// Saces imported airports to JSON file.
        /// </summary>
        /// <param name="absolutePath">Absolute path to the file.</param>
        /// <param name="airports">The airports to save.</param>
        /// <returns><see langword="true"/> is successful, otherwise <see langword="false"/>.</returns>
        bool SaveAirportsToJson(string absolutePath, List<ImportedAirport> airports);
    }
}
