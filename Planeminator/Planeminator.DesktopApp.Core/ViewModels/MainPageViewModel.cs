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
        public string Seed { get; set; }

        public string AirportsPath { get; set; }

        public string AirportsCount => Airports?.Count.ToString() ?? "0";

        public List<CheckableImportedAirport> Airports { get; set; }

        public SimulationReport Report { get; set; }

        #region Commands 

        public ICommand ImportAirportsCommand { get; set; }

        public ICommand SaveAirportsCommand { get; set; }

        public ICommand StartSimulationCommand { get; set; }

        #endregion

        private readonly IFileDialogService mFileService;
        private readonly IAirportImportService mAirpotImporter;
        private readonly IMapper mMapper;

        public MainPageViewModel()
        {

        }

        public MainPageViewModel(IFileDialogService fileService, IAirportImportService airpotImporter, IMapper mapper)
        {
            mFileService = fileService;
            mAirpotImporter = airpotImporter;
            mMapper = mapper;
            ImportAirportsCommand = new RelayCommand(ImportAirports);
            SaveAirportsCommand = new RelayCommand(SaveAirports);
            StartSimulationCommand = new RelayCommand(StartSimulation);
        }

        private async void StartSimulation()
        {
            var planes = mMapper.Map<List<Airport>>(Airports);
            using var scope = Framework.Container.BeginLifetimeScope();
            var builder = scope.Resolve<ISimulationBuilder>();
            builder
                .WithPlanes(new List<Plane>()
                {
                    new Plane()
                    {
                        Model = "Antonov 1",
                        Mileague = new LinearMileageFunction(20, 10),
                    },
                    new Plane()
                    {
                        Model = "Antonov 2",
                        Mileague = new LinearMileageFunction(18, 5),
                    },
                    new Plane()
                    {
                        Model = "Antonov 3",
                        Mileague = new LinearMileageFunction(22, 4),
                    },
                    new Plane()
                    {
                        Model = "Antonov 4",
                        Mileague = new LinearMileageFunction(11, 4),
                    },
                    new Plane()
                    {
                        Model = "Antonov 5",
                        Mileague = new LinearMileageFunction(33, 2),
                    },
                    new Plane()
                    {
                        Model = "Antonov 6",
                        Mileague = new LinearMileageFunction(20, 10),
                    },
                    new Plane()
                    {
                        Model = "Antonov 7",
                        Mileague = new QuadraticMileageFunction(10, 10, 10),
                    },
                    new Plane()
                    {
                        Model = "Antonov 8",
                        Mileague = new QuadraticMileageFunction(20, 10, 20),
                    },
                    new Plane()
                    {
                        Model = "Antonov 9",
                        Mileague =  new QuadraticMileageFunction(5, 5, 20),
                    },
                    new Plane()
                    {
                        Model = "Antonov 10",
                        Mileague =  new QuadraticMileageFunction(7, 15, 14),
                    },
                })
                .WithAirports(planes);

            if (int.TryParse(Seed, out var seedInt))
                builder.WithSeed(seedInt);

            var simulation = builder.Build();

            if (await simulation.StartAsync())
            {
                Report = simulation.GetReport();
            }
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
