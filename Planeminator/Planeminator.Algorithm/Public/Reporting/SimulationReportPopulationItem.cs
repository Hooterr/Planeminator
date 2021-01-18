using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportPopulationItem
    {
        public double ObjectiveFunctionValue { get; set; }
        public List<SimulationReportIterationPlane> Planes { get; set; }
    }
}
