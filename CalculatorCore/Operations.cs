// <copyright file="Operations.cs" company="Ivan Yankov">
// Copyright (c) Ivan Yankov. All rights reserved.
// </copyright>
// <author>Ivan Yankov</author>
namespace CalculatorCore
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Performs various math operations.
    /// </summary>
    public static class Operations
    {
        /// <summary>
        /// Performs a sum of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The sum of the two numbers.</returns>
        public static double Sum(float a, float b)
        {
            var result = Convert.ToDouble(a + b);
            return result;
        }

        /// <summary>
        /// Performs a subtraction of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The subtraction result of the two numbers.</returns>
        public static double Subtract(float a, float b)
        {
            var result = Convert.ToDouble(a - b);
            return result;
        }

        /// <summary>
        /// Performs a multiplication of two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The multiplication result of the two numbers.</returns>
        public static double Multiply(float a, float b)
        {
            var result = Convert.ToDouble(a * b);
            return result;
        }

        /// <summary>
        /// Performs a two numbers division.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The division result of the two numbers.</returns>
        public static double Divide(float a, float b)
        {
            var result = Convert.ToDouble(a / b);
            return result;
        }

        /// <summary>
        /// Computes a given math expression, passed as string.
        /// </summary>
        /// <param name="expression">The math expression to compute.</param>
        /// <returns>The result of the performed calculations.</returns>
        /// <exception cref="System.Data.EvaluateException">Thrown, when illegal expression is passed.</exception>
        public static double Compute(string expression)
        {
            TranslateCustomOperators(ref expression, true);

            // Translating the user defined functions, because the data table does not understands them.
            TranslateCustomFunctions(ref expression);
            using (var dataTable = new DataTable())
            {
                // Replacing the decimal separator, because the data table requires '.' separator, regardless the current culture.
                var result = dataTable.Compute(expression.Replace(',', '.'), string.Empty);

                return Convert.ToDouble(result);
            }
        }

        /// <summary>
        /// Pre-calculates the extra functions (defined at the <see cref="Custom.Functions"/> class) for a given math expression.
        /// </summary>
        /// <param name="expression">The math expression to translate.</param>
        private static void TranslateCustomFunctions(ref string expression)
        {
            var copy = expression;
            var functionName = expression.GetRootFunctionName();
            if (functionName == null)
            {
                // The expression does not contains custom function.
                return;
            }

            var extracted = ExtractFunctions(expression);
            foreach (var item in extracted)
            {
                if (item.ArgumentValue.HasValue)
                {
                    // The function argument is already simplified.
                    expression = expression.Replace(item.Argument, item.ArgumentValue.Value.ToString());
                }
                else
                {
                    // This function must be simplified.
                    expression = expression.Replace(item.Expression, item.Simplify());
                }
            }

            var expressions = expression.Split(
                new string[] { " + ", " - ", " * ", " / " },
                StringSplitOptions.RemoveEmptyEntries);

            var operators = expression.Split(expressions, StringSplitOptions.RemoveEmptyEntries);
            var newExpressions = new List<string>();

            var customFunctionNames = Custom.Functions.Keys;
            foreach (var expr in expressions)
            {
                var fname = expr.GetRootFunctionName();
                if (fname != null)
                {
                    var customExpr = new CustomExpression(expr);
                    var simplified = customExpr.Simplify();
                    newExpressions.Add(simplified);
                }
                else
                {
                    newExpressions.Add(expr);
                }
            }

            if (newExpressions.Count - 1 != operators.Length)
            {
                throw new InvalidProgramException(
                    "The simplification of the expression failed!");
            }

            // Building the output expression
            string simplifiedExpression = string.Empty;
            for (int i = 0; i < newExpressions.Count; i++)
            {
                simplifiedExpression += newExpressions[i];
                if (i < operators.Length)
                {
                    simplifiedExpression += operators[i];
                }
            }

            expression = simplifiedExpression;
        }

        /// <summary>
        /// Extracts custom expressions from a string expression.
        /// </summary>
        /// <param name="expression">The input expression represented as text.</param>
        /// <returns>List of custom expressions.</returns>
        private static List<CustomExpression> ExtractFunctions(string expression)
        {
            var results = new List<CustomExpression>();

            // Analyzing braces
            Stack<int> openingBraces = new Stack<int>();
            var braces = new Dictionary<int, int>();
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    openingBraces.Push(i);
                }
                else if (expression[i] == ')')
                {
                    if (openingBraces.Count != 0)
                    {
                        braces.Add(openingBraces.Pop(), i);
                    }
                }
            }

            int index = 0;
            string functionName = string.Empty;

            while (index < expression.Length)
            {
                var trimmed = expression.Substring(index);
                functionName = trimmed.GetRootFunctionName();
                if (functionName == null)
                {
                    break;
                }

                index = expression.IndexOf(functionName, index);
                if (index == -1)
                {
                    break;
                }

                int openBracketIndex = index + functionName.Length;

                if (braces.ContainsKey(openBracketIndex))
                {
                    int closeBracketIndex = braces[openBracketIndex];
                    var subExpr = expression.Substring(index, closeBracketIndex + 1 - index);
                    results.Add(new CustomExpression(subExpr));
                    index = closeBracketIndex;
                }
                else
                {
                    index += functionName.Length;
                }
            }

            return results;
        }

        /// <summary>
        /// Pre-calculates the extra operators (defined at the <see cref="Custom.LeftAssociativeOperators"/> and <see cref="Custom.RightAssociativeOperators"/> for a given math expression.
        /// </summary>
        /// <param name="expression">The math expression to translate.</param>
        /// <param name="rightAssociative">Determines the operator associativity.</param>
        private static void TranslateCustomOperators(ref string expression, bool rightAssociative)
        {
            char? @operator = expression.GetOperator(rightAssociative);
            if (@operator == null)
            {
                return;
            }

            int operatorIndex;
            if (rightAssociative)
            {
                operatorIndex = expression.LastIndexOf(@operator.Value);
            }
            else
            {
                operatorIndex = expression.IndexOf(@operator.Value);
            }

            int leftIndex = operatorIndex - 1;
            int rightIndex = operatorIndex + 1;
            double? left = null;
            double? right = null;
            int replaceLeftIndex = -1;
            int replaceRightIndex = -1;
            bool isLeftSideExpression = false;
            bool hasLeftSideOperator = false;
            bool isRightSideExpression = false;
            bool hasRightSideOperator = false;

            while (leftIndex >= 0)
            {
                var ls = expression.Substring(leftIndex, operatorIndex - leftIndex).Replace(" ", string.Empty);
                if (ls.Contains(')'))
                {
                    isLeftSideExpression = true;
                }

                if (ls.Contains('+') || ls.Contains('-') || ls.Contains('*') || ls.Contains('/'))
                {
                    hasLeftSideOperator = true;
                }

                if (hasLeftSideOperator && !isLeftSideExpression)
                {
                    break;
                }

                double? computed = null;
                try
                {
                    computed = Operations.Compute(ls);
                    left = computed.Value;
                    replaceLeftIndex = leftIndex;
                }
                catch (Exception)
                {
                    if (left != null)
                    {
                        break;
                    }
                }

                leftIndex--;
            }

            while (rightIndex < expression.Length)
            {
                var rs = expression.Substring(operatorIndex + 1, rightIndex - operatorIndex).Replace(" ", string.Empty);
                if (rs.Contains(')'))
                {
                    isRightSideExpression = true;
                }

                if (rs.Contains('+') || rs.Contains('-') || rs.Contains('*') || rs.Contains('/'))
                {
                    hasRightSideOperator = true;
                }

                if (hasRightSideOperator && !isRightSideExpression)
                {
                    break;
                }

                double? computed = null;
                try
                {
                    computed = Operations.Compute(rs);
                    right = computed.Value;
                    replaceRightIndex = rightIndex;
                }
                catch (Exception)
                {
                    if (right != null)
                    {
                        break;
                    }
                }

                rightIndex++;
            }

            if (left != null && right != null)
            {
                var textToReplace = expression.Substring(replaceLeftIndex, replaceRightIndex - replaceLeftIndex + 1);
                string result;
                if (rightAssociative)
                {
                    result = Custom.RightAssociativeOperators[@operator.Value].Invoke(left.Value, right.Value).ToString();
                }
                else
                {
                    result = Custom.LeftAssociativeOperators[@operator.Value].Invoke(left.Value, right.Value).ToString();
                }

                expression = expression.Replace(textToReplace, result);
                TranslateCustomOperators(ref expression, rightAssociative);
            }
        }
    }
}