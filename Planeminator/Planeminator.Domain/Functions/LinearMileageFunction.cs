using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Functions
{
    public class LinearMileageFunction : IMileageFunction
    {
        public double A { get; set; }
        public double B { get; set; }

        /// <summary>
        /// Creates a new linear mileage function in a form of y=ax+b
        /// </summary>
        /// <param name="a">a coefficient in a form of liters per kg per km.</param>
        /// <param name="b">b coefficient in a form of liters per kg per km.</param>
        public LinearMileageFunction(double a, double b)
        {
            A = a;
            B = b;
        }

        public double CalculateMailage(double payloadMassKg)
        {
            return payloadMassKg * A + B;
        }
    }
}
