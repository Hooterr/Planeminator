using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Planeminator.Algorithm.Services
{
    internal class SimulationImplementation : Simulation
    {
        [Description("k")]
        public const double PriceFulePerLiter = 5;


        private readonly IAirportDistanceMap DistanceMap;
        private readonly List<AlgorithmAirport> Airports;
        private readonly List<AlgorithmPlane> Planes;
        private readonly Random rng;

        public SimulationImplementation(List<AlgorithmAirport> airports, List<AlgorithmPlane> planes, int seed) : this(airports, planes)
        {
            rng = new Random(seed);
        }

        public SimulationImplementation(List<AlgorithmAirport> airports, List<AlgorithmPlane> planes)
        {
            DistanceMap = AirportDistanceMap.FromAirports(airports);
            Airports = airports;
            Planes = planes;
            rng ??= new Random();
        }

        private int CurrentTimeUnitNumber;

        public override async Task<bool> Start()
        {
            await Task.Run(() =>
            {
                CurrentTimeUnitNumber = 0;
                InitializeSimulation();

                for(int i = 0; i < 10; i++)
                {
                    // 1. Unload packages from plane
                    UnloadPackagesFromPlanes();

                    // 2. IYKWIM
                    OptimizeRoutes();

                    // 3. Fly plane to the next locations
                    FlyPlanes();

                    // 4. Update the time unit
                    CurrentTimeUnitNumber++;
                }

                return true;
            });

            return true;
        }

        private void OptimizeRoutes()
        {
            var gettingBetter = true;

            // Generate random routes
            GenerateRandomRoutesForAllPlanes();

            while (gettingBetter)
            {
                // Temporarily assign packages onto planes
                TemporaryLoadPackagesOntoPlanes();

                // Do stuff...
                OptimazeRoutesMutateGenesOrDoWhateverIDontCare();

                
                // Briliant stop condition goes here...
                gettingBetter = false;
            }
        }

        private void OptimazeRoutesMutateGenesOrDoWhateverIDontCare()
        {
            // Sick algorithm goes here...
            var objFctnValue = CalculateObjectiveFunction();
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
            return plane.Mileague.CalculateMailage(plane.Packages.Sum(package => package.MassKg)) * PriceFulePerLiter * DistanceMap.GetDistance(plane.CurrentAirport, destination);
        }

        private void UnloadPackagesFromPlanes()
        {
            Airports.ForEach(airport =>
            {
                airport.AvailablePlanes.ForEach(plane =>
                {
                    var undeliveredPackages = plane.Packages
                        .Where(x => x.Destination != airport)
                        .ToList();

                    airport.Packages.AddRange(undeliveredPackages);

                    // For reporting later
                    var delievered = plane.Packages
                        .Except(undeliveredPackages)
                        .ToList();

                    plane.Packages.Clear();
                });
            });
        }

        /// <summary>
        /// Temporarily loads the packages on the planes.
        /// <para>During this assignemnt packages are <b>not</b> removed from the airport's package pool.</para>
        /// </summary>
        private void TemporaryLoadPackagesOntoPlanes()
        {
            AssignPackagesToPlanes(temporarily: true);
        }

        /// <summary>
        /// Pernamently loads the packages on the planes.
        /// <para>During this assignemnt packages <b>are</b> removed from the airport's package pool.</para>
        /// </summary>
        private void LoadPackgesOntoPlanesFinal()
        {
            AssignPackagesToPlanes(temporarily: false);
        }

        private void AssignPackagesToPlanes(bool temporarily)
        {
            Airports.ForEach(airport =>
            {
                airport.Packages.ForEach(package =>
                {
                    var planeToLoadThePackageOnto =
                        airport.AvailablePlanes
                        .Select(plane => new
                        {
                            plane,
                            distanceToDestinationInRoute = plane.Route.IndexOf(package.Destination),
                        })
                        .Where(x => x.distanceToDestinationInRoute != -1)
                        .OrderBy(x => x.distanceToDestinationInRoute)
                        .Select(x => x.plane)
                        .FirstOrDefault();

                    if (planeToLoadThePackageOnto != null)
                    {
                        planeToLoadThePackageOnto.Packages.Add(package);

                        if (temporarily == false)
                            airport.Packages.Remove(package);
                    }
                });
            });
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

            GenerateNewPackagesForAiports();
        }

        private void GenerateNewPackagesForAiports()
        {
            foreach(var airport in Airports)
            {
                var nrPckgs = rng.Next(1, 5);
                var newPackages = new List<AlgorithmPackage>(nrPckgs);
                for (var i = 0; i < nrPckgs; i++)
                {
                    var package = new AlgorithmPackage()
                    {
                        DeadlineInTimeUnits = rng.Next(2, 5),
                        Income = rng.Next(1, 10),
                        MassKg = rng.NextDouble() * 10d + 0.01d,
                        Origin = airport,
                    };

                    while(package.Destination == null || package.Destination == package.Origin)
                    {
                        package.Destination = GetRandomAirport();
                    }

                    newPackages.Add(package);
                }

                airport.Packages.AddRange(newPackages);
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


        #endregion
    }
}
