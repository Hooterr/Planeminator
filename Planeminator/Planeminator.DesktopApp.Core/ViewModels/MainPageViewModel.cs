using AutoMapper;
using Planeminator.Algorithm.Public;
using Planeminator.DataIO.Public.Models;
using Planeminator.DataIO.Public.Services;
using Planeminator.DesktopApp.Core.Models;
using Planeminator.DesktopApp.Core.Services;
using Planeminator.DesktopApp.Core.ViewModels.Base;
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

        public ICommand ImportAirportsCommand { get; set; }

        public ICommand SaveAirportsCommand { get; set; }

        public ICommand StartSimulationCommand { get; set; }


        private readonly IFileDialogService mFileService;
        private readonly IAirportImportService mAirpotImporter;
        private readonly IMapper mMapper;
        private readonly ISimulationBuilder builder;

        public MainPageViewModel()
        {

        }

        public MainPageViewModel(IFileDialogService fileService, IAirportImportService airpotImporter, IMapper mapper, ISimulationBuilder simBuilder)
        {
            mFileService = fileService;
            mAirpotImporter = airpotImporter;
            mMapper = mapper;
            builder = simBuilder;
            ImportAirportsCommand = new RelayCommand(ImportAirports);
            SaveAirportsCommand = new RelayCommand(SaveAirports);
            StartSimulationCommand = new RelayCommand(StartSimulation);
        }

        private void StartSimulation()
        {
            var simulation = builder
                .WithPlanes(new List<Plane>()
                {
                    new Plane()
                    {
                        Model = "Antonov",
                        //Mileague =
                                 
                    }
                })
                .WithAirports(new List<Airport>()
                {
                    new Airport()
                    {

                    }
                })
                .Build();
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
