﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReportRoundIterationItem
    {
        public readonly int Number;

        public double ObjectiveFunctionValue { get; set; }

        public List<SimulationReportRoundPlane> Planes { get; set; }

        public SimulationReportRoundIterationItem(int number)
        {
            Number = number;
        }
    }
}