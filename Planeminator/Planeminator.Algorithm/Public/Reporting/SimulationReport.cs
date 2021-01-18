using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReport
    {
        public List<SimulationReportRoundIterationItem> Iterations { get; set; }

        public SimulationReportPopulationItem FinalSolution { get; set; }

    }
}
