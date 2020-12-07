using Autofac;
using AutoMapper;
using MathNet.Numerics.Distributions;
using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public;
using Planeminator.Algorithm.Public.Reporting;
using Planeminator.Domain.DI;
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
        private readonly double FuelPricePerLiter;
        private readonly int DurationInUnits;

        private readonly IAirportDistanceMap DistanceMap;
        private readonly List<AlgorithmAirport> Airports;
        private readonly List<AlgorithmPlane> Planes;
        private readonly List<AlgorithmPackage> UndelieveredPackages = new List<AlgorithmPackage>();
        private readonly Random rng;
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
            reporting.WithAiports(Airports).WithPlanes(Planes).FinishInit();
            packageGenerator = PackageGenerator.WithSettings(settings.PackageGeneration, rng);
            DurationInUnits = settings.DurationInTimeUnits;
            FuelPricePerLiter = settings.FuelPricePerLiter;
        }

        private int CurrentTimeUnitNumber;
        public override async Task<bool> StartAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    CurrentTimeUnitNumber = 0;
                    InitializeSimulation();

                    for (int i = 0; i < DurationInUnits; i++)
                    {
                        progress(CurrentTimeUnitNumber + 1, 0);
                        
                        // 1. Generate new packages
                        GenerateNewPackagesForAiports();

                        // 2. Unload existing packages from plane
                        UnloadPackagesFromPlanes();

                        // 3. IYKWIM
                        OptimizeRoutes();

                        // 4. Fly plane to the next locations
                        FlyPlanes();

                        // 5. Update Packages deadlines
                        UpdateDeadlines();

                        // 6. Update the time unit
                        CurrentTimeUnitNumber++;
                        reporting.NextRound();
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }

                return true;
            });

            return true;
        }

        private void UpdateDeadlines()
        {
            UndelieveredPackages.ForEach(pack => pack.DeadlineLeftTimeUnits--);
        }

        private void OptimizeRoutes()
        {
            var gettingBetter = true;

            // Generate random routes
            GenerateRandomRoutesForAllPlanes();
            var itn = 0;
            while (gettingBetter)
            {
                progress(CurrentTimeUnitNumber + 1, itn + 1);

                // Reload packages onto planes <---- this one takes the most time
                LoadPackgesOntoPlanesFinal();

                // Do stuff...
                OptimazeRoutesMutateGenesOrDoWhateverIDontCare();

                // Briliant stop condition goes here...
                gettingBetter = rng.NextDouble() > 0.5;

                if (gettingBetter)
                    reporting.NextIteration();

                itn++;
            }
        }

        private void OptimazeRoutesMutateGenesOrDoWhateverIDontCare()
        {
            // Sick algorithm goes here...
            var objFctnValue = CalculateObjectiveFunction();

            reporting.ReportIterationFinish(objFctnValue);
        }

        private void GenerateRandomRoutesForAllPlanes()
        {
            Planes.ForEach(plane =>
            {
                plane.Route = Enumerable
                                .Range(0, AlgorithmPlane.RouteLength)
                                .Select(x => GetRandomAirport())
                                .ToList();

            });
        }

        private void FlyPlanes()
        {
            Planes.ForEach(plane =>
            {
                // Get the next destination airport on the route
                var destinationAirport = plane.Route.First();

                // Check if it is different the the current airport
                if (destinationAirport != plane.CurrentAirport)
                {
                    // Remove the plane from its current airport
                    plane.CurrentAirport.AvailablePlanes.Remove(plane);
                    // And add it to the destination airport
                    destinationAirport.AvailablePlanes.Add(plane);
                }
            });
        }

        private double CalculateObjectiveFunction()
        {
            return Planes.Sum(plane => 
                plane.Route.Sum(routeItem => 
                    -GetFlightCost(plane, routeItem) + plane.Packages.Sum(package =>  /* TBD Start */package.Destination == routeItem ? package.Income : 0 - 0  /* TBD End */)));
        }


        private double GetFlightCost(AlgorithmPlane plane, AlgorithmAirport destination)
        {
            //                                           Improve complexity by caching total package weight
            return plane.Mileague.CalculateMailage(plane.Packages.Sum(package => package.MassKg)) * FuelPricePerLiter * DistanceMap.GetDistance(plane.CurrentAirport, destination);
        }

        private void UnloadPackagesFromPlanes()
        {
            Airports.ForEach(airport =>
            {
                airport.AvailablePlanes.ForEach(plane =>
                {
                    plane.Packages.ForEach(package =>
                    {
                        // Delievered
                        if (package.Destination == airport)
                        {
                            // Remove it from delievered
                            UndelieveredPackages.Remove(package);
                        }
                        // Undelievered
                        else
                        {
                            // Add it to the airport pool
                            airport.Packages.Add(package);
                        }
                    });

                    // Unload everything from the plane
                    plane.Packages.Clear();
                });
            });
        }

        /// <summary>
        /// Temporarily loads the packages on the planes.
        /// <para>During this assignemnt packages are <b>not</b> removed from the airport's package pool.</para>
        /// </summary>
        private void ReloadPackagesOntoPlanes()
        {
            // Clear all packages from planes
            Planes.ForEach(plane => plane.Packages.Clear());

            // And assign them again
            Airports.ForEach(airport =>
            {
                airport.Packages.ForEach(package =>
                {
                    var planeToLoadThePackageOnto = ChooseAirportForPackage(package, airport.AvailablePlanes);

                    if (planeToLoadThePackageOnto != null)
                    {
                        planeToLoadThePackageOnto.Packages.Add(package);
                    }
                });
            });
        }

        /// <summary>
        /// Pernamently loads the packages on the planes.
        /// <para>During this assignemnt packages <b>are</b> removed from the airport's package pool.</para>
        /// </summary>
        private void LoadPackgesOntoPlanesFinal()
        {
            // And assign them again
            Airports.ForEach(airport =>
            {
                airport.Packages.ForEach(package =>
                {
                    var planeToLoadThePackageOnto = ChooseAirportForPackage(package, airport.AvailablePlanes);

                    if (planeToLoadThePackageOnto != null)
                    {
                        planeToLoadThePackageOnto.Packages.Add(package);

                        // If uncommented throws CollectionModifiedException
                        //airport.Packages.Remove(package);
                    }
                });
                airport.AvailablePlanes.SelectMany(x => x.Packages).ToList().ForEach(package => airport.Packages.Remove(package));
            });
        }

        private AlgorithmPlane ChooseAirportForPackage(AlgorithmPackage package, List<AlgorithmPlane> availablePlanes)
        {
            var planeToLoadThePackageOnto =
                availablePlanes
                .Select(plane => new
                {
                    plane,
                    distanceToDestinationInRoute = plane.Route.IndexOf(package.Destination),
                })
                .Where(x => x.distanceToDestinationInRoute != -1)
                .OrderBy(x => x.distanceToDestinationInRoute)
                .Select(x => x.plane)
                .FirstOrDefault();

            return planeToLoadThePackageOnto;
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
