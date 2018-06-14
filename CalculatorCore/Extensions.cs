// <copyright file="Extensions.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace CalculatorCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides various extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Finds the root function name of a given math expression.
        /// </summary>
        /// <param name="mathExpression">The math expression to check.</param>
        /// <returns>Returns null if nothing found.</returns>
        public static string GetRootFunctionName(this string mathExpression)
        {
            if (mathExpression == null)
            {
                throw new ArgumentNullException("mathExpression");
            }

            var dict = new Dictionary<int, string>();
            foreach (var customFunctionName in Custom.Functions.Keys)
            {
                int index = mathExpression.IndexOf(customFunctionName);
                if (index != -1)
                {
                    dict.Add(index, customFunctionName);
                }
            }

            if (dict.Count == 0)
            {
                return null;
            }
            else
            {
                int indexOfFunctionName = dict.Min(x => x.Key);
                var rootFunctionName = dict[indexOfFunctionName];
                return rootFunctionName;
            }
        }

        /// <summary>
        /// Finds the left or right associative operator from a given math expression.
        /// </summary>
        /// <param name="mathExpression">The math expression to check.</param>
        /// <param name="isRightAssociative">Determines the operator associativity.</param>
        /// <returns>Returns null if nothing found.</returns>
        public static char? GetOperator(this string mathExpression, bool isRightAssociative)
        {
            if (mathExpression == null)
            {
                throw new ArgumentNullException("mathExpression");
            }

            var dict = new Dictionary<int, char>();
            foreach (var customOperator in Custom.RightAssociativeOperators.Keys)
            {
                int index;
                if (isRightAssociative)
                {
                    index = mathExpression.LastIndexOf(customOperator);
                }
                else
                {
                    index = mathExpression.IndexOf(customOperator);
                }

                if (index != -1)
                {
                    dict.Add(index, customOperator);
                }
            }

            if (dict.Count == 0)
            {
                return null;
            }
            else
            {
                int indexOfOperator;
                if (isRightAssociative)
                {
                    indexOfOperator = dict.Max(x => x.Key);
                }
                else
                {
                    indexOfOperator = dict.Min(x => x.Key);
                }

                var rootOperator = dict[indexOfOperator];
                return rootOperator;
            }
        }
    }
}