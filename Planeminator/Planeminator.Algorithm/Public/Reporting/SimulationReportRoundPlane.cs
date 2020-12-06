using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportRoundPlane
    {
        public List<SimulationReportAirport> Route { get; set; }

        public Plane UnderlyingPlane { get; set; }
    }
}
