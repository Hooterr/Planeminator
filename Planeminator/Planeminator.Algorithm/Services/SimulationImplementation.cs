using Planeminator.Algorithm.DataStructures;
using Planeminator.Algorithm.Public;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeminator.Algorithm.Services
{
    internal class SimulationImplementation : Simulation
    {
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

                for (int i = 0; i < 10; i++)
                {
                    // 1. Unload packages from plane
                    UnloadPackagesFromPlanes();

                    // 3. Choose and load packages onto the planes
                    LoadPackagesOntoPlanes();

                    // 4. Fly plane to the next locations
                    FlyPlanes();

                    // 5. Update the day
                    CurrentTimeUnitNumber++;
                }

                return true;
            });

            return true;
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

        private void LoadPackagesOntoPlanes()
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
                        airport.Packages.Remove(package);
                    }
                });
            });
        }

        private void InitializeSimulation()
        {
            // Assign random planes to random airports and generate random route
            Planes.ForEach(plane =>
            {
                var airport = GetRndomAirport();
                airport.AvailablePlanes.Add(plane);
                plane.CurrentAirport = airport;

                // Random routes for all planes
                plane.Route = Enumerable.Range(0, AlgorithmPlane.RouteLength)
                                .Select(x => GetRndomAirport())
                                .ToList();
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
                        package.Destination = GetRndomAirport();
                    }

                    newPackages.Add(package);
                }

                airport.Packages.AddRange(newPackages);
            }
        }

        #region Helpers

        private AlgorithmAirport GetRndomAirport()
        {
            return Airports[rng.Next(0, Airports.Count - 1)];
        }

        private AlgorithmPlane GetRndomPlane()
        {
            return Planes[rng.Next(0, Planes.Count - 1)];
        }


        #endregion
    }
}
