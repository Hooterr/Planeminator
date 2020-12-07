using MathNet.Numerics.Distributions;
using Planeminator.Algorithm.Public;
using System;

namespace Planeminator.Algorithm.DataStructures
{
    internal class NormalDistributionRandomNumberProvider : IRandomNumberProvider
    {
        private readonly Normal distr;
        private readonly double minValue;

        public NormalDistributionRandomNumberProvider(double mean, double stddev, Random random) : this(mean, stddev, random, double.MinValue)
        { }

        public NormalDistributionRandomNumberProvider(double mean, double stddev, Random random, double minValue)
        {
            distr = Normal.WithMeanStdDev(mean, stddev, random);
            this.minValue = minValue;
        }

        public double Next()
        {
            double value;
            do
            {
                value = distr.Sample();
            } while (value < minValue);

            return value;
        }

        public static IRandomNumberProvider WithSettings(AlgorithmNormalDistributionParameters parameters, Random random)
        {
            return new NormalDistributionRandomNumberProvider(parameters.Mean, parameters.StandardDeviation, random, parameters.MinValue);
        }
    }
}
