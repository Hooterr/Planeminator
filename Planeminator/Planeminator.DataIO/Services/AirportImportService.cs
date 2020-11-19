using AutoMapper;
using Newtonsoft.Json;
using Planeminator.DataIO.Import.Models;
using Planeminator.DataIO.Public.Models;
using Planeminator.DataIO.Public.Services;
using System;
using System.Collections.Generic;
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
                throw new Exception("Cannot import provided file. Look at inner exception for more information", ex);
            }
        }
    }
}
