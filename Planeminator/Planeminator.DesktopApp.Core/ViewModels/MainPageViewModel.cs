using Autofac;
using AutoMapper;
using OfficeOpenXml;
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
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Planeminator.DesktopApp.Core.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public string Seed { get; set; } = "123456";

        public string AirportsPath { get; set; }

        public string AirportsCount => Airports?.Count.ToString() ?? "0";

        public List<CheckableImportedAirport> Airports { get; set; }

        public SimulationReport Report { get; set; }

        public double? FinalSolutionObjValue => Report?.FinalSolution.ObjectiveFunctionValue;

        public string PopulationSeed { get; set; } = "654321";

        public int Iteration { get; set; }

        [DependsOn(nameof(Iteration), nameof(IterationsTotal))]
        public double Percentage => 100 * (double)Iteration / (IterationsTotal == 0 ? 1 : IterationsTotal);

        public bool Running { get; set; }

        public bool NotRunning => !Running;

        public string FuelPrice { get; set; } = "0.1";
        public int IterationsTotal { get; set; }
        public string NumberOfIterations { get; set; } = "15";

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

        public string Penalty { get; set; } = "5";

        public string NumberOfGenerations { get; set; } = "200";
        public string MutationProbability { get; set; } = "0.001";

        public string NumberOfPlanes { get; set; } = "20";

        #region Commands 

        public ICommand ImportAirportsCommand { get; set; }

        public ICommand SaveAirportsCommand { get; set; }

        public ICommand StartSimulationCommand { get; set; }

        public ICommand ExportExcelCommand { get; set; }

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
            ExportExcelCommand = new RelayCommand(ExportExcel);
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
                    Planes = new List<Plane>(),
                    Airports = airports,
                    DurationInTimeUnits = int.Parse(NumberOfIterations),
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

                if (int.TryParse(Seed, out var seedInt))
                    builder.Settings.Seed = seedInt;
                else
                    builder.Settings.Seed = null;

                if (int.TryParse(PopulationSeed, out seedInt))
                    builder.Settings.InitialPopulationSeed = seedInt;
                else
                    builder.Settings.InitialPopulationSeed = null;

                var numOfPlanes = int.Parse(NumberOfPlanes);
                builder.Settings.Planes.AddRange(Enumerable.Range(1, numOfPlanes).Select(i => new Plane()
                {
                    Model = "Plane " + i,
                    Mileague = new LinearMileageFunction(a, b),
                }));

                builder.Settings.PenaltyPercent = double.Parse(Penalty);
                builder.Settings.NumberOfIterations = int.Parse(NumberOfIterations);
                builder.Settings.MutationProbability = double.Parse(MutationProbability);
                builder.Settings.GenerationSize = int.Parse(NumberOfGenerations);
                IterationsTotal = builder.Settings.NumberOfIterations;

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

        private void UpdateProgress(int interation)
        {
            Iteration = interation;
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

        private void ExportExcel()
        {
            if (Report == null)
                return;
            try
            {
                using ExcelPackage excel = new ExcelPackage();
                excel.Workbook.Worksheets.Add("data");

                var outputFileName = mFileService.SaveToFile("Excel file (*.xlsx) | *.xlsx");

                if (string.IsNullOrEmpty(outputFileName))
                    return;

                var genCount = Report.Iterations.Select(x => x.GenerationsCount).Max();

                string[] row1 = new List<string>(){ "Iteration\\Generation" }.Concat(Enumerable.Range(1, genCount).Select(x => x.ToString())).ToArray();
                List<string[]> headerRow = Enumerable.Range(1, Report.Iterations.Count).Select(x => new string[] { x.ToString() }).ToList();
                headerRow = headerRow.Prepend(row1).ToList();

                // Determine the header range
                string headerRange = "A1:" + GetExcelColumnName(headerRow[0].Length) + "1";
                string rowRange = "A1:" + GetExcelColumnName(headerRow[0].Length) + "1";

                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["data"];

                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[2, 2].LoadFromArrays(Report.Iterations.Select(x => x.Generations.Select(x => (object)x.ObjectiveFunctionValue).ToArray()));
                FileInfo excelFile = new FileInfo(outputFileName);
                excel.SaveAs(excelFile);
            }
            catch (Exception ex)
            {
                mUI.ShowInfo(ex.ToString(), "Error");
            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
