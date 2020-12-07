using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportRoundPlane
    {
        public List<SimulationReportAirport> Route { get; set; }

        public List<SimulationReportPackage> Packages { get; set; }

        public IEnumerable<object> AllDisplayableCollections => new List<object>() { Route, Packages };

        public Plane UnderlyingPlane { get; set; }
    }
}
