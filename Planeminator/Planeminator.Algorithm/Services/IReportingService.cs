using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.DataStructures.Genetic;
using Planeminator.Algorithm.Public.Reporting;
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

        IReportingService NextIteration();

        IReportingService ReportNewPackagesAdded(IEnumerable<AlgorithmPackage> packages);

        IReportingService NextPopulation(Population population);

        IReportingService FinalSolution(Population final);

        SimulationReport Finish();
    }
}
