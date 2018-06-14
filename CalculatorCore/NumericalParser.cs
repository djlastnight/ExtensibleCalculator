// <copyright file="NumericalParser.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace CalculatorCore
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Static class, which is used to parse numbers.
    /// </summary>
    public static class NumericalParser
    {
        /// <summary>
        /// Converts the string representation of a number to a double. 
        /// </summary>
        /// <param name="input">A string containing a number to convert.</param>
        /// <returns>Null if the operation fails.</returns>
        public static double? ParseDouble(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            double result;
            bool parseOK = double.TryParse(
                input,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CurrentCulture,
                out result);

            if (parseOK)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
