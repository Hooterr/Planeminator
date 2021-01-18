using Autofac;
using AutoMapper;
using MathNet.Numerics.Distributions;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.DataStructures.Genetic;
using Planeminator.Algorithm.Public;
using Planeminator.Algorithm.Public.Reporting;
using Planeminator.Domain.DI;
using Planeminator.Domain.Extensions;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Planeminator.Algorithm.Services
{
    internal class SimulationImplementation : Simulation
    {
        private readonly int PopulationSize;
        private readonly double MutationProbability;
        private readonly int MutationProbabilityRngRange;
        private readonly double FuelPricePerLiter;
        private readonly double PenaltyPerDayPercentage;
        private readonly int IterationCount;
        private List<Population> Generations;
        private readonly List<AlgorithmAirport> Airports;
        private readonly List<AlgorithmPlane> Planes;
        private readonly List<AlgorithmPackage> UndelieveredPackages = new List<AlgorithmPackage>();
        private readonly Random rng;
        private readonly IAirportDistanceMap DistanceMap;
        private readonly IMapper mapper;
        private readonly IReportingService reporting;
        private readonly IPackageGenerator packageGenerator;
        private readonly ProgressHandler progress;

        public SimulationImplementation(SimulationSettings settings, ProgressHandler handler)
        {

            using var scope = Framework.Container.BeginLifetimeScope();
            mapper = scope.Resolve<IMapper>();
            reporting = scope.Resolve<IReportingService>();

            progress = handler;
            var algAirports = mapper.Map<List<AlgorithmAirport>>(settings.Airports);
            var algPlanes = mapper.Map<List<AlgorithmPlane>>(settings.Planes);

            var idCurrent = 0;
            foreach (var airport in algAirports)
            {
                airport.InternalId = idCurrent++;
            }

            idCurrent = 0;
            foreach (var plane in algPlanes)
            {
                plane.InternalId = idCurrent++;
            }

            rng = settings.Seed.HasValue ? new Random(settings.Seed.Value) : new Random();
            DistanceMap = AirportDistanceMap.FromAirports(algAirports);
            Airports = algAirports;
            Planes = algPlanes;
            IterationCount = settings.NumberOfIterations;
            reporting.WithAiports(Airports).WithPlanes(Planes).FinishInit();
            packageGenerator = PackageGenerator.WithSettings(settings.PackageGeneration, rng);
            FuelPricePerLiter = settings.FuelPricePerLiter;
            PopulationSize = settings.GenerationSize;
            MutationProbability = settings.MutationProbability;
            MutationProbabilityRngRange = (int)(100 / MutationProbability);

            if (settings.PenaltyPercent <= 0)
                throw new ArgumentException(nameof(settings.PenaltyPercent));

            PenaltyPerDayPercentage = settings.PenaltyPercent;
        }

        public override async Task<bool> StartAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    InitializeSimulation();
    
                    GenerateNewPackagesForAiports();

                    OptimizeRoutes();
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }

                return true;
            });

            return true;
        }

        private void OptimizeRoutes()
        {
            var itn = 0;
            var mask = (byte)rng.Next();
            CreateGeneration();

            while (true)
            {
                progress(itn + 1);

                // 1. Objective function calculation
                Generations.ForEach(element =>
                {
                    element.ObjectiveFunctionValue = CalculateObjectiveFunction(element.Elements);
                    reporting.NextPopulation(element);
                });

                // Report this
                reporting.NextIteration();

                // 2. Check stop condition
                if (itn + 1 >= IterationCount)
                {
                    goto finish;
                }

                // 3. Selection
                var selected = Generations
                    .Select((element, index) => new { element, groupId = rng.Next(0, (int)Math.Floor(Generations.Count * 0.3d)) })
                    .GroupBy(x => x.groupId)
                    .Select(x => x.Aggregate((i1, i2) => i1.element.ObjectiveFunctionValue > i2.element.ObjectiveFunctionValue ? i1 : i2))
                    .Select(x => x.element)
                    .ToList();

                // 4. Pairing
                var pairs = new List<Population[]>();
                while (pairs.Count < Generations.Count / 2)
                {
                    var idx1 = rng.Next(0, selected.Count);
                    var idx2 = rng.Next(0, selected.Count);
                    while (idx1 != idx2)
                        idx2 = rng.Next(0, selected.Count);

                    var first = selected[idx1];
                    var second = selected[idx2];
                    pairs.Add(new Population[]{ first, second });
                }

                // 5. Crossing
                var newGeneration = new List<Population>();
                foreach(var pair in pairs)
                {
                    var first = pair[0].Elements;
                    var second = pair[1].Elements;
                    var newFirst = new Population();
                    var newSecond = new Population();

                    for(int i = 0; i < first.Count(); i++)
                    {
                        var firstElement = first[i];
                        var secondElement = second[i];
                        var newFirstElement = new PopulationElement()
                        {
                            Plane = firstElement.Plane,
                        }; 
                        var newSecondElement = new PopulationElement()
                        {
                            Plane = secondElement.Plane,
                        };

                        byte cnt = 1;
                        for (int j = 0; j < firstElement.Route.Count(); j++)
                        {
                            var newFirstRouteItem = (mask & cnt) > 1 ? firstElement.Route[j] : secondElement.Route[j];
                            var newSecondRouteItem = (mask & cnt) == 0 ? firstElement.Route[j] : secondElement.Route[j];
                            newFirstElement.Route.Add(newFirstRouteItem);
                            newSecondElement.Route.Add(newSecondRouteItem);
                            cnt <<= 1;
                        }

                        newFirst.Elements.Add(newFirstElement);
                        newSecond.Elements.Add(newSecondElement);
                    }

                    newGeneration.Add(newFirst);
                    newGeneration.Add(newSecond);
                }

                // 6. Mutation
                newGeneration.SelectMany(x => x.Elements).ForEach(x =>
                {
                    if (RngMutation())
                    {
                        var swap1 = rng.Next(0, x.Route.Count());
                        var swap2 = swap1;
                        while(swap1 == swap2)
                            swap2 = rng.Next(0, x.Route.Count());

                        x.Route.Swap(swap1, swap2);
                    }
                });

                // 7. Replace population
                if (newGeneration.Count == 0)
                    newGeneration.AddRange(selected);

                Generations = newGeneration;

                if (Generations.Count == 1)
                    goto finish;
                else if (Generations.Count == 0)
                    throw new Exception("Generations count has reached 0");

                itn++;
            }

        finish:
            var final = Generations.OrderBy(x => x.ObjectiveFunctionValue).FirstOrDefault();
            CalculateObjectiveFunction(final.Elements);
            reporting.FinalSolution(final);
            reporting.NextPopulation(final);
        }
        
        private bool RngMutation()
        {
            return rng.Next(0, MutationProbabilityRngRange) == 0;
        }

        private void CreateGeneration()
        {
            Generations = new List<Population>();
            for(int i = 0; i < PopulationSize; i++)
            {
                var populationElement = new Population()
                {
                    Elements = Planes.Select(plane => new PopulationElement()
                    {
                        Plane = plane,
                        Route = Enumerable
                            .Range(0, AlgorithmPlane.RouteLength)
                            .Select(i => GetRandomAirport()).ToList(),
                    }).ToList(),
                };

                Generations.Add(populationElement);
            }
        }

        private double CalculateObjectiveFunction(List<PopulationElement> population)
        {
            // Load packages since we need them to calculate obj function
            LoadPackagesOntoPlanes(population);

            var objFcnValue = 0d;

            // Iterate over planes in population
            foreach(var element in population)
            {
                var plane = element.Plane;

                var currentPlaneMass = plane.Packages.Sum(x => x.MassKg);
                var currentRouteItem = plane.CurrentAirport;
                for (int t = 0; t < element.Route.Count(); t++)
                {
                    var nextRouteItem = element.Route[t];
                    var packagesDelieveredThisRouteItem = plane.Packages.Where(package => element.Route.IndexOf(package.Destination) == t).ToList();
                    objFcnValue += -plane.Mileague.CalculateMailage(currentPlaneMass) * FuelPricePerLiter * DistanceMap.GetDistance(currentRouteItem, nextRouteItem);
                    objFcnValue += packagesDelieveredThisRouteItem.Sum(package => package.Income * PenaltyFactor(package, element.Route.IndexOf(package.Destination) + 1));
                    currentPlaneMass -= packagesDelieveredThisRouteItem.Sum(x => x.MassKg);
                    currentRouteItem = nextRouteItem;
                }

            }

            return objFcnValue;
        }

        private double PenaltyFactor(AlgorithmPackage package, int delieverdIn)
        {
            return Math.Min(1, 1 - ((delieverdIn - package.DeadlineInTimeUnits) * PenaltyPerDayPercentage / 100d));
        }


        /// <summary>
        /// Temporarily loads the packages on the planes.
        /// <para>During this assignemnt packages are <b>not</b> removed from the airport's package pool.</para>
        /// </summary>
        private void LoadPackagesOntoPlanes(List<PopulationElement> population)
        {
            var planes = population.Select(x => x.Plane);
            
            // Clear all packages from planes
            foreach (var plane in planes)
                plane.Packages.Clear();

            // And assign them again
            Airports.ForEach(airport =>
            {
                airport.Packages.ForEach(package =>
                {
                    var availablePlanes = population.Where(x => x.Plane.CurrentAirport == airport);
                    var planeToLoadThePackageOnto = ChooseAirportForPackage(package, availablePlanes);

                    if (planeToLoadThePackageOnto != null)
                    {
                        planeToLoadThePackageOnto.Packages.Add(package);
                    }
                });
            });
        }

        private AlgorithmPlane ChooseAirportForPackage(AlgorithmPackage package, IEnumerable<PopulationElement> availablePlanes)
        {
            var a = availablePlanes.ToList();
            var planeToLoadThePackageOnto =
                availablePlanes
                .Where(element => element.Route.Contains(package.Destination))
                .OrderBy(plane => plane.Route.IndexOf(package.Destination))
                .FirstOrDefault();
             
            return planeToLoadThePackageOnto?.Plane;
        }

        private void InitializeSimulation()
        {
            // Assign random planes to random airports
            Planes.ForEach(plane =>
            {
                var airport = GetRandomAirport();
                airport.AvailablePlanes.Add(plane);
                plane.CurrentAirport = airport;
            });
        }

        private void GenerateNewPackagesForAiports()
        {
            foreach(var airport in Airports)
            {
                var newPackages = packageGenerator.NextPool();

                foreach (var package in newPackages)
                {
                    package.Origin = airport;

                    while (package.Destination == null || package.Destination == package.Origin)
                    {
                        package.Destination = GetRandomAirport();
                    }
                }

                airport.Packages.AddRange(newPackages);
                UndelieveredPackages.AddRange(newPackages);
                reporting.ReportNewPackagesAdded(newPackages);
            }
        }

        #region Helpers

        private AlgorithmAirport GetRandomAirport()
        {
            return Airports[rng.Next(0, Airports.Count - 1)];
        }

        private AlgorithmPlane GetRandomPlane()
        {
            return Planes[rng.Next(0, Planes.Count - 1)];
        }

        public override SimulationReport GetReport()
        {
            return reporting.Finish();
        }

        #endregion
    }
}
