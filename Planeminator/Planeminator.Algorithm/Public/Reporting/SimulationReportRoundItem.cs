using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportRoundItem
    {
        public int RoundNumber { get; private set; }

        public List<SimulationReportRoundIterationItem> Iterations { get; set; }

        public SimulationReportRoundIterationItem FinalSolution => Iterations.LastOrDefault();

        public SimulationReportRoundItem(int roundNumber)
        {
            RoundNumber = roundNumber;
        }

    }
}
