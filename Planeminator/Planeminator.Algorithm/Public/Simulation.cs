using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planeminator.Algorithm.Public
{
    public abstract class Simulation
    {
        public abstract Task<bool> StartAsync();

        protected Simulation() { }
    }
}
