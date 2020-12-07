using Planeminator.Algorithm.Public.Reporting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Planeminator.DesktopApp.Converters
{
    public class ListOfObjectsToListOfSimReportAirportListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is List<object> valueObjList)
            {
                return valueObjList.Cast<SimulationReportAirport>();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
