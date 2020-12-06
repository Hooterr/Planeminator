using Planeminator.Domain.Interfaces;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportPackage : IPackage<SimulationReportAirport>
    {
        public double Income { get; set; }
        public double MassKg { get; set; }
        public int DeadlineInTimeUnits { get; set; }
        public SimulationReportAirport Origin { get; set; }
        public SimulationReportAirport Destination { get; set; }
    }
}
