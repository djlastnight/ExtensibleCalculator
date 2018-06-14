// <copyright file="Custom.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace CalculatorCore
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A set of user defined stuff.
    /// </summary>
    public static class Custom
    {
        /// <summary>
        /// Represents a collection (dictionary) of user defined calculator functions.
        /// <para>The key represents the function name. The value is delegate, which performs the calculation.</para>
        /// </summary>
        public static readonly Dictionary<string, Func<double, double>> Functions;

        /// <summary>
        /// Represents a collection (dictionary) of user defined calculator operators.
        /// <para>The key represents the operator. The value is delegate, which performs the calculation.</para>
        /// </summary>
        public static readonly Dictionary<char, Func<double, double, double>> LeftAssociativeOperators;

        /// <summary>
        /// Represents a collection (dictionary) of user defined calculator operators.
        /// <para>The key represents the operator. The value is delegate, which performs the calculation.</para>
        /// </summary>
        public static readonly Dictionary<char, Func<double, double, double>> RightAssociativeOperators;

        /// <summary>
        /// Initializes static members of the <see cref="Custom"/> class.
        /// </summary>
        static Custom()
        {
            Functions = new Dictionary<string, Func<double, double>>();
            Functions.Add("sqrt", (number) => { return Math.Sqrt(number); });
            Functions.Add("reciproc", (number) => { return 1.0 / number; });
            Functions.Add("negate", (number) => { return number * -1; });
            Functions.Add("sind", (number) => { return Math.Sin(Math.PI * number / 180.0); });
            Functions.Add("cosd", (number) => { return Math.Cos(Math.PI * number / 180.0); });
            Functions.Add(
                "tand",
                (number) =>
            {
                var sin = Math.Sin(Math.PI * number / 180.0);
                var cos = Math.Cos(Math.PI * number / 180.0);
                int check = (int)(cos * Math.Pow(10, 14));
                if (check == 0)
                {
                    throw new InvalidOperationException("Invalid input");
                }

                return sin / cos;
            });

            Functions.Add(
                "log",
                (number) =>
            {
                if (number < 0)
                {
                    throw new InvalidOperationException("Invalid input");
                }

                return Math.Log10(number);
            });

            Functions.Add("grad2deg", (number) => { return number * 360.0 / 400.0; });
            Functions.Add("rad2deg", (number) => { return 180 * number / Math.PI; });

            /*
             * According to the current implementation
             * the user defined operators always have higher precedence
             * than the 'standard' operators like '+', '-', '*', '/'
             */
            LeftAssociativeOperators = new Dictionary<char, Func<double, double, double>>();

            RightAssociativeOperators = new Dictionary<char, Func<double, double, double>>();
            RightAssociativeOperators.Add('^', (a, b) => { return Math.Pow(a, b); });
        }
    }
}
