using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Interfaces
{
    public interface IDeliverable<TAirport>
    {
        public int DeadlineInTimeUnits { get; set; }

        public TAirport Origin { get; set; }

        public TAirport Destination { get; set; }
    }
}
