using Planeminator.Algorithm.Public.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Planeminator.DesktopApp.DataTemplateSelectors
{
    public class SimulationReportPlanePropsDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                if (item is List<SimulationReportAirport>)
                    return element.FindResource("routeTemplate") as DataTemplate;
                else if (item is List<SimulationReportPackage>)
                    return element.FindResource("packageTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
