using AutoMapper;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public.Reporting;
using Planeminator.Domain.Models;

namespace Planeminator.Algorithm.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Airport, AlgorithmAirport>()
                .ForMember(dst => dst.AssociatedAirport, opt => opt.MapFrom(src => src));

            CreateMap<Plane, AlgorithmPlane>()
                .ForMember(dst => dst.AssociatedPlane, opt => opt.MapFrom(src => src));

            CreateMap<AlgorithmAirport, SimulationReportAirport>()
                .ForMember(dst => dst.UnderlyingAirport, opt => opt.MapFrom(src => src.AssociatedAirport));

            CreateMap<AlgorithmPackage, SimulationReportPackage>()
                .ForMember(dst => dst.Origin, opt => opt.Ignore())
                .ForMember(dst => dst.Destination, opt => opt.Ignore());

            //CreateMap<AlgorithmPlane, SimulationReportRoundPlane>()
            //    .ForMember(dst => dst.UnderlyingPlane, opt => opt.MapFrom(src => src.AssociatedPlane))
            //    .ForMember(dst => dst.Route, opt => opt.Ignore());
        }
    }
}
