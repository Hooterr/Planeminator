using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Interfaces
{
    public interface IPackage<TAirport> : IDeliverable<TAirport>
    {
        public double Income { get; set; }

        public double MassKg { get; set; }
    }
}
