using Planeminator.Domain.Interfaces;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public.Reporting
{
    public class SimulationReport
    {
        public List<SimulationReportRoundItem> Rounds { get; set; }
    }
}
