using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.Public
{
    public class PackageGenerationSettings
    {
        public AlgorithmNormalDistributionParameters MassDistribution { get; set; }
        public AlgorithmNormalDistributionParameters IncomeDistribution { get; set; }
        public AlgorithmNormalDistributionParameters DeadlineDistribution { get; set; }
        public AlgorithmNormalDistributionParameters CountDistribution { get; set; }
    }
}
