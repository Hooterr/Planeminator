using Autofac;
using Planeminator.DataIO.Public.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Planeminator.DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using var scope = App.Container.BeginLifetimeScope();
            var airportImportService = scope.Resolve<IAirportImportService>();
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "airports.json");
            var test = airportImportService.ImportAirportsFromJson(path);
        }
    }
}
