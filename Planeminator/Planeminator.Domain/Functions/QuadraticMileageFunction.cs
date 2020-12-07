using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Functions
{
    public class QuadraticMileageFunction : IMileageFunction
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        /// <summary>
        /// Creates a new linear mileage function in a form of y=ax+b
        /// </summary>
        /// <param name="a">a coefficient in a form of liters per kg per km.</param>
        /// <param name="b">b coefficient in a form of liters per kg per km.</param>
        /// <param name="c">c coefficient in a form of liters per kg per km.</param>
        public QuadraticMileageFunction(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public double CalculateMailage(double payloadMassKg)
        {
            return Math.Pow(payloadMassKg, 2) * A + payloadMassKg * B + C;
        }
    }
}
