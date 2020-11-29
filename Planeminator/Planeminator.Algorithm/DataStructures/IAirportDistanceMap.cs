using Planeminator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.DataStructures
{
    internal interface IAirportDistanceMap
    {
        double GetDistance(IAirport from, IAirport to);
    }
}
