using AutoMapper;
using Geolocation;
using Planeminator.DataIO.Import.Models;
using Planeminator.DataIO.Public.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DataIO.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<AirportListItem, ImportedAirport>()
                .ForMember(dest => dest.Coordinate, opt => opt.MapFrom(source => 
                    new Coordinate() { Latitude = source.Geolocation.Latitude, Longitude = source.Geolocation.Longitude }));
        }
    }
}
