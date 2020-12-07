using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportRoundIterationItem
    {
        public int Number { get; private set; }

        public double ObjectiveFunctionValue { get; set; }

        public List<SimulationReportRoundPlane> Planes { get; set; }

        public SimulationReportRoundIterationItem(int number)
        {
            Number = number;
        }
    }
}
