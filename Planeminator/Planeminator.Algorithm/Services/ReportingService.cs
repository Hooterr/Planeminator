using AutoMapper;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public.Reporting;
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
        //private Dictionary<AlgorithmPlane, SimulationReportRoundPlane> planeMapping;

        private int currentRoundNumber = 0;
        private int currentIterationNumber = 0;

        private readonly SimulationReport report;

        private SimulationReportRoundItem CurrentRound => report.Rounds.ElementAtOrDefault(currentRoundNumber);

        private SimulationReportRoundIterationItem CurrentIteration => CurrentRound?.Iterations.ElementAtOrDefault(currentIterationNumber);

        public ReportingService(IMapper mapper)
        {
            this.mapper = mapper;
            report = new SimulationReport()
            {
                Rounds = new List<SimulationReportRoundItem>()
                {
                    new SimulationReportRoundItem(currentRoundNumber)
                    {
                        Iterations = new List<SimulationReportRoundIterationItem>()
                        {
                            new SimulationReportRoundIterationItem(currentIterationNumber)
                        }
                    }
                }
            };
        }

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

            //var reportPlanes = mapper.Map<List<SimulationReportRoundPlane>>(planes);
            //planeMapping = new Dictionary<AlgorithmPlane, SimulationReportRoundPlane>(planes.Zip(reportPlanes, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v));

            initialized = true;
            return this;
        }

        public IReportingService NextRound()
        {
            if (!initialized)
                throw new InvalidOperationException();

            currentRoundNumber++;
            currentIterationNumber = 0;
            report.Rounds.Add(new SimulationReportRoundItem(currentRoundNumber)
            {
                Iterations = new List<SimulationReportRoundIterationItem>()
                {
                    new SimulationReportRoundIterationItem(currentIterationNumber)
                }
            });
            return this;
        }

        public IReportingService NextIteration()
        {
            if (!initialized)
                throw new InvalidOperationException(); 
            
            currentIterationNumber++;
            CurrentRound.Iterations.Add(new SimulationReportRoundIterationItem(currentIterationNumber));
            return this;
        }

        public IReportingService Report(double objectiveFunctionValie)
        {
            if (!initialized)
                throw new InvalidOperationException(); 
            
            CurrentIteration.ObjectiveFunctionValue = objectiveFunctionValie;
            CurrentIteration.Planes = planes.Select(plane => new SimulationReportRoundPlane()
            {
                UnderlyingPlane = plane,
                Route = plane.Route.Select(algAiport => airportMapping[algAiport]).ToList()
            }).ToList();
            
            return this;
        }
    }
}
