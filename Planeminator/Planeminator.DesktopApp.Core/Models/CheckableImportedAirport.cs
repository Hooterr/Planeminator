using Newtonsoft.Json;
using Planeminator.DataIO.Public.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DesktopApp.Core.Models
{
    public class CheckableImportedAirport : ImportedAirport
    {
        [JsonIgnore]
        public bool IsChecked { get; set; }
    }
}
