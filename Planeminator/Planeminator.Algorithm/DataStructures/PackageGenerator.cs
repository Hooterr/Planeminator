using Planeminator.Algorithm.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.DataStructures
{
    internal class PackageGenerator : IPackageGenerator
    {
        private readonly IRandomNumberProvider MassProvider;
        private readonly IRandomNumberProvider IncomeProvider;
        private readonly IRandomNumberProvider DeadlineProvider;
        private readonly IRandomNumberProvider PoolSizeProvider;

        private int LastAssignedId = 0;

        public PackageGenerator(IRandomNumberProvider massProvider, IRandomNumberProvider incomeProvider, IRandomNumberProvider deadlineProvider, IRandomNumberProvider poolSizeProvider)
        {
            MassProvider = massProvider;
            IncomeProvider = incomeProvider;
            DeadlineProvider = deadlineProvider;
            PoolSizeProvider = poolSizeProvider;
        }

        public List<AlgorithmPackage> NextPool()
        {
            var poolSize = (int)PoolSizeProvider.Next();
            var result = new List<AlgorithmPackage>(poolSize);
            while (poolSize > 0)
            {
                var package = new AlgorithmPackage()
                {
                    Id = ++LastAssignedId,
                    MassKg = MassProvider.Next(),
                    Income = IncomeProvider.Next(),
                    DeadlineInTimeUnits = (int)DeadlineProvider.Next(),
                };

                poolSize--;
                result.Add(package);
            }

            return result;
        }

        public static IPackageGenerator WithSettings(PackageGenerationSettings settings, Random random)
        {
            if (settings == null)
                throw new ArgumentException(nameof(settings));

            // Mass
            if (settings.MassDistribution == null)
                throw new ArgumentNullException(nameof(settings.MassDistribution));

            if (settings.MassDistribution.MinValue <= 0)
                throw new ArgumentException(nameof(settings.MassDistribution));

            var massProv = NormalDistributionRandomNumberProvider.WithSettings(settings.MassDistribution, random);

            // Income
            if (settings.IncomeDistribution == null)
                throw new ArgumentNullException(nameof(settings.IncomeDistribution));

            if (settings.IncomeDistribution.MinValue <= 0)
                throw new ArgumentException(nameof(settings.IncomeDistribution));

            var incomeProv = NormalDistributionRandomNumberProvider.WithSettings(settings.IncomeDistribution, random);

            // Deadline
            if (settings.DeadlineDistribution == null)
                throw new ArgumentNullException(nameof(settings.DeadlineDistribution));

            if (settings.DeadlineDistribution.MinValue <= 0)
                throw new ArgumentException(nameof(settings.DeadlineDistribution));

            var deadlineProv = NormalDistributionRandomNumberProvider.WithSettings(settings.DeadlineDistribution, random);

            // Count
            if (settings.CountDistribution == null)
                throw new ArgumentNullException(nameof(settings.CountDistribution));

            if (settings.CountDistribution.MinValue < 0)
                throw new ArgumentException(nameof(settings.CountDistribution));

            var countProv = NormalDistributionRandomNumberProvider.WithSettings(settings.CountDistribution, random);

            // Generator
            return new PackageGenerator(massProv, incomeProv, deadlineProv, countProv);
        }

    }
}
