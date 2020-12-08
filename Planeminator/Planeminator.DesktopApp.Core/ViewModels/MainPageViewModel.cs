using Autofac;
using AutoMapper;
using Planeminator.Algorithm.Public;
using Planeminator.Algorithm.Public.Reporting;
using Planeminator.DataIO.Public.Models;
using Planeminator.DataIO.Public.Services;
using Planeminator.DesktopApp.Core.Models;
using Planeminator.DesktopApp.Core.Services;
using Planeminator.DesktopApp.Core.ViewModels.Base;
using Planeminator.Domain.DI;
using Planeminator.Domain.Functions;
using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Planeminator.DesktopApp.Core.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public string Seed { get; set; } = 123456.ToString();

        public string AirportsPath { get; set; }

        public string AirportsCount => Airports?.Count.ToString() ?? "0";

        public List<CheckableImportedAirport> Airports { get; set; }

        public SimulationReport Report { get; set; }

        public int TotalRounds { get; set; } = 30;

        public int Iteration { get; set; }

        public int Round { get; set; }

        public double Percentage { get; set; }

        public bool Running { get; set; }

        public bool NotRunning => !Running;

        public string FuelPrice { get; set; } = "0.1";

        public string NumberOfRounds { get; set; } = "30";

        public string PackageMassMean { get; set; } = "10";
        public string PackageMassStd { get; set; } = "5";
        public string PackageMassMin { get; set; } = "0.001";


        public string PackageIncomeMean { get; set; } = "10";
        public string PackageIncomeStd { get; set; } = "5";
        public string PackageIncomeMin { get; set; } = "0.001";


        public string PackageDeadlineMean { get; set; } = "10";
        public string PackageDeadlineStd { get; set; } = "5";
        public string PackageDeadlineMin { get; set; } = "0.001";


        public string PackageCountMean { get; set; } = "10";
        public string PackageCountStd { get; set; } = "5";
        public string PackageCountMin { get; set; } = "0.001";

        public string Mileage { get; set; } = "2.45";
        #region Commands 

        public ICommand ImportAirportsCommand { get; set; }

        public ICommand SaveAirportsCommand { get; set; }

        public ICommand StartSimulationCommand { get; set; }

        #endregion

        private readonly IFileDialogService mFileService;
        private readonly IAirportImportService mAirpotImporter;
        private readonly IMapper mMapper;
        private readonly IUIManager mUI;

        public MainPageViewModel()
        {

        }

        public MainPageViewModel(IFileDialogService fileService, IAirportImportService airpotImporter, IMapper mapper, IUIManager UI)
        {
            mFileService = fileService;
            mAirpotImporter = airpotImporter;
            mMapper = mapper;
            mUI = UI;
            ImportAirportsCommand = new RelayCommand(ImportAirports);
            SaveAirportsCommand = new RelayCommand(SaveAirports);
            StartSimulationCommand = new RelayCommand(StartSimulation);
        }

        private async void StartSimulation()
        {
            try
            {
                var airports = mMapper.Map<List<Airport>>(Airports);
                var builder = SimulationBuilder.New();
                var mileague = double.Parse(Mileage);
                var b = 1.5;
                var a = mileague / 100;
                builder.Settings = new SimulationSettings()
                {
                    Planes = new List<Plane>()
                    {
                        new Plane()
                        {
                            Model = "Antonov 1",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 2",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 3",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 4",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 5",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 6",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 7",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 8",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 9",
                            Mileague = new LinearMileageFunction(a, b),
                        },
                        new Plane()
                        {
                            Model = "Antonov 10",
                            Mileague = new LinearMileageFunction(a, b)
                        },
                    },
                    Airports = airports,
                    DurationInTimeUnits = int.Parse(NumberOfRounds),
                    FuelPricePerLiter = double.Parse(FuelPrice),
                    PackageGeneration = new PackageGenerationSettings()
                    {
                        CountDistribution = new AlgorithmNormalDistributionParameters()
                        {
                            Mean = double.Parse(PackageCountMean),
                            StandardDeviation = double.Parse(PackageCountStd),
                            MinValue = double.Parse(PackageCountMin),
                        },
                        DeadlineDistribution = new AlgorithmNormalDistributionParameters()
                        {
                            Mean = double.Parse(PackageDeadlineMean),
                            StandardDeviation = double.Parse(PackageDeadlineStd),
                            MinValue = double.Parse(PackageDeadlineMin),
                        },
                        IncomeDistribution = new AlgorithmNormalDistributionParameters()
                        {
                            Mean = double.Parse(PackageIncomeMean),
                            StandardDeviation = double.Parse(PackageIncomeStd),
                            MinValue = double.Parse(PackageIncomeMin),
                        },
                        MassDistribution = new AlgorithmNormalDistributionParameters()
                        {
                            Mean = double.Parse(PackageMassMean),
                            StandardDeviation = double.Parse(PackageMassStd),
                            MinValue = double.Parse(PackageMassMin),
                        }
                    }
                };
                TotalRounds = int.Parse(NumberOfRounds);

                if (int.TryParse(Seed, out var seedInt))
                    builder.Settings.Seed = seedInt;
                else
                    builder.Settings.Seed = null;

                builder.ProgressHandler = UpdateProgress;

                var simulation = builder.Build();

                if (await simulation.StartAsync())
                {
                    Report = simulation.GetReport();
                }
            }
            catch (Exception ex)
            {
                mUI.ShowInfo(ex.ToString(), "Error");
                Debugger.Break();
            }
        }

        private void UpdateProgress(int round, int interation)
        {
            Percentage = (double)round / TotalRounds * 100;
            Iteration = interation;
            Round = round;
        }

        private void SaveAirports()
        {
            var @checked = Airports.Where(x => x.IsChecked).Cast<ImportedAirport>().ToList();
            var path = mFileService.SaveToFile("Json file(*.json) | *.json");

            if (string.IsNullOrEmpty(path))
                return;

            var result = mAirpotImporter.SaveAirportsToJson(path, @checked);

            if (result == false)
                Debugger.Break();
        }

        private void ImportAirports()
        {
            var file = mFileService.BrowseForFile("Json file (*.json) | *.json");
            if (string.IsNullOrEmpty(file))
                return;

            AirportsPath = file;
            Airports = mMapper.Map<List<CheckableImportedAirport>>(mAirpotImporter.ImportAirportsFromJson(file));
        }
    }
}
