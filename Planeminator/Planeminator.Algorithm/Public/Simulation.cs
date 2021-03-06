﻿using Planeminator.Algorithm.Public.Reporting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Planeminator.Algorithm.Public
{
    public abstract class Simulation
    {
        public abstract Task<bool> StartAsync();

        public abstract SimulationReport GetReport();

        protected Simulation() { }
    }
}
