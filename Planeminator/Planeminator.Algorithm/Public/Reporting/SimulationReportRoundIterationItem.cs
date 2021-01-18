using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportRoundIterationItem
    {
        public int Number { get; private set; }

        public List<SimulationReportPopulationItem> Generations { get; set; }

        public int GenerationsCount => Generations?.Count ?? 0;

        public SimulationReportRoundIterationItem(int number)
        {
            Number = number;
            Generations = new List<SimulationReportPopulationItem>();
        }
    }
}
