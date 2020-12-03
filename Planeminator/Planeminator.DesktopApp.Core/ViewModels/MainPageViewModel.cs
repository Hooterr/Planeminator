using Planeminator.DataIO.Public.Models;
using Planeminator.DataIO.Public.Services;
using Planeminator.DesktopApp.Core.Services;
using Planeminator.DesktopApp.Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace Planeminator.DesktopApp.Core.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public string Seed { get; set; }

        public string AirportsPath { get; set; }

        public string AirportsCount => Airports?.Count.ToString() ?? "0";

        public List<ImportedAirport> Airports { get; set; }

        public ICommand ImportAirportsCommand { get; set; }

        private readonly IFileDialogService mFileService;
        private readonly IAirportImportService mAirpotImporter;

        public MainPageViewModel()
        {

        }

        public MainPageViewModel(IFileDialogService fileService, IAirportImportService airpotImporter)
        {
            mFileService = fileService;
            mAirpotImporter = airpotImporter;
            ImportAirportsCommand = new RelayCommand(ImportAirports);
        }

        private void ImportAirports()
        {
            var file = mFileService.BrowseForFile("Json file (*.json) | *.json");
            if (string.IsNullOrEmpty(file))
                return;

            AirportsPath = file;
            Airports= mAirpotImporter.ImportAirportsFromJson(file);
        }
    }
}
