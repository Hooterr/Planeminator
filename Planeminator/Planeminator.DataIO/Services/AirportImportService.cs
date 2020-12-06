using AutoMapper;
using Newtonsoft.Json;
using Planeminator.DataIO.Import.Models;
using Planeminator.DataIO.Public.Models;
using Planeminator.DataIO.Public.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Planeminator.DataIO.Services
{
    internal class AirportImportService : IAirportImportService
    {
        private readonly IMapper _mapper;

        public AirportImportService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<ImportedAirport> ImportAirportsFromJson(string absolutePath)
        {
            try
            {
                using (var file = File.OpenText(absolutePath))
                {
                    var serializer = new JsonSerializer();
                    var items = (List<AirportListItem>)serializer.Deserialize(file, typeof(List<AirportListItem>));

                    var result = _mapper.Map<List<AirportListItem>, List<ImportedAirport>>(items);

                    return result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Cannot import provided file. Look at the inner exception for more information", ex);
            }
        }

        public bool SaveAirportsToJson(string absolutePath, List<ImportedAirport> airports)
        {
            try
            {
                using (var streamWriter = File.CreateText(absolutePath))
                {
                    var items = _mapper.Map<List<ImportedAirport>, List<AirportListItem>>(airports);
                    var serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, items);
                }
                return true;
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
            {
                Debugger.Break();
                return false;
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }
    }
}
