using AutoMapper;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.DataStructures.Genetic;
using Planeminator.Algorithm.Public.Reporting;
using Planeminator.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planeminator.Algorithm.Services
{
    internal class ReportingService : IReportingService
    {
        private bool initialized = false;
        private readonly IMapper mapper;
        private IReadOnlyList<AlgorithmAirport> airports;
        private IReadOnlyList<AlgorithmPlane> planes;

        private Dictionary<AlgorithmAirport, SimulationReportAirport> airportMapping;
        private Dictionary<AlgorithmPackage, SimulationReportPackage> packageMapping;

        private int currentIterationNumber = 0;
        private int currentPopulationNumber = -1;

        private readonly SimulationReport report;

        private SimulationReportRoundIterationItem CurrentIteration => report.Iterations.ElementAtOrDefault(currentIterationNumber);
        private SimulationReportPopulationItem CurrentPopulation => CurrentIteration.Generations.ElementAt(currentPopulationNumber);

        public ReportingService(IMapper mapper)
        {
            this.mapper = mapper;
            report = new SimulationReport()
            {
                Iterations = new List<SimulationReportRoundIterationItem>()
                {
                    new SimulationReportRoundIterationItem(currentIterationNumber)
                }
            };
        }

        #region Init

        public IReportingService WithAiports(IReadOnlyList<AlgorithmAirport> airports)
        {
            if (initialized)
                throw new InvalidOperationException();

            this.airports = airports;
            return this;
        }

        public IReportingService WithPlanes(IReadOnlyList<AlgorithmPlane> planes)
        {
            if (initialized)
                throw new InvalidOperationException();

            this.planes = planes;
            return this;
        }

        public IReportingService FinishInit()
        {
            if (initialized)
                throw new InvalidOperationException();

            var reportAirports = mapper.Map<List<SimulationReportAirport>>(airports);
            airportMapping = new Dictionary<AlgorithmAirport, SimulationReportAirport>(airports.Zip(reportAirports, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v));
            packageMapping = new Dictionary<AlgorithmPackage, SimulationReportPackage>();

            initialized = true;
            return this;
        }

        #endregion

        public IReportingService NextIteration()
        {
            if (!initialized)
                throw new InvalidOperationException(); 
            
            currentIterationNumber++;
            report.Iterations.Add(new SimulationReportRoundIterationItem(currentIterationNumber));
            return this;
        }

        public IReportingService NextPopulation(Population population)
        {
            if (!initialized)
                throw new InvalidOperationException();

            CurrentIteration.Generations.Add(new SimulationReportPopulationItem()
            {
                ObjectiveFunctionValue = population.ObjectiveFunctionValue,
                Planes = population.Elements.Select(element => new SimulationReportIterationPlane()
                {
                    UnderlyingPlane = element.Plane,
                    Route = element.Route.Select(algAiport => airportMapping[algAiport]).ToList(),
                    Packages = element.Plane.Packages.Select(algPackage => packageMapping[algPackage]).ToList(),
                }).ToList(),
            });
            
            currentPopulationNumber++;

            return this;
        }
         
        public IReportingService FinalSolution(Population final)
        {
            report.FinalSolution = new SimulationReportPopulationItem()
            {
                ObjectiveFunctionValue = final.ObjectiveFunctionValue,
                Planes = final.Elements.Select(element => new SimulationReportIterationPlane()
                {
                    UnderlyingPlane = element.Plane,
                    Route = element.Route.Select(algAiport => airportMapping[algAiport]).ToList(),
                    Packages = element.Plane.Packages.Select(algPackage => packageMapping[algPackage]).ToList(),
                }).ToList(),
            };

            return this;
        }

        public IReportingService ReportNewPackagesAdded(IEnumerable<AlgorithmPackage> packages)
        {
            packages.ForEach(algPackage =>
            {
                var repPackage = mapper.Map<SimulationReportPackage>(algPackage);
                repPackage.Origin = airportMapping[algPackage.Origin];
                repPackage.Destination = airportMapping[algPackage.Destination];
                packageMapping[algPackage] = repPackage;
            });

            return this;
        }

        public SimulationReport Finish()
        {
            return report;
        }

    }
}
