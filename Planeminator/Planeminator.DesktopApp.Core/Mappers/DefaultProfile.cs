using AutoMapper;
using Planeminator.DataIO.Public.Models;
using Planeminator.DesktopApp.Core.Models;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DesktopApp.Core.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<ImportedAirport, CheckableImportedAirport>()
                .ReverseMap();
            CreateMap<ImportedAirport, Airport>()
                .ReverseMap();
        }
    }
}
