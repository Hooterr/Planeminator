using AutoMapper;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Domain.Models;

namespace Planeminator.Algorithm.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Airport, AlgorithmAirport>();
            CreateMap<Plane, AlgorithmPlane>();
        }
    }
}
