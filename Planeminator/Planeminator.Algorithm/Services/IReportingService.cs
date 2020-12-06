using Planeminator.Algorithm.DataStructures;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Services
{
    internal interface IReportingService
    {
        IReportingService WithPlanes(IReadOnlyList<AlgorithmPlane> planes);

        IReportingService WithAiports(IReadOnlyList<AlgorithmAirport> airports);

        IReportingService FinishInit();

        IReportingService NextRound();

        IReportingService NextIteration();

        IReportingService Report(double objectiveFunctionValie);
    }
}
