// <copyright file="CustomExpression.cs" company="Ivan Yankov">
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
    /// Represents a custom math expression, which could be simplified.
    /// </summary>
    public class CustomExpression
    {
        /// <summary>
        /// Hold a value indicating the start replace index of the original math expression.
        /// </summary>
        private int replaceStartIndex = 0;

        /// <summary>
        /// Hold a value indicating the end replace index of the original math expression.
        /// </summary>
        private int replaceEndIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExpression"/> class from a given expression.
        /// </summary>
        /// <param name="expression">The custom math expression.</param>
        /// <exception cref="ArgumentNullException">Thrown, when the passed argument is null.</exception>
        /// <exception cref="ArgumentException">Thrown, when the expression is invalid.</exception>
        public CustomExpression(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.Name = expression.GetRootFunctionName();
            if (this.Name == null)
            {
                throw new ArgumentException(
                    "The expression does not contain a custom function!");
            }

            int indexOfFunctionName = expression.IndexOf(this.Name);
            this.Expression = expression;

            if (this.Expression[indexOfFunctionName + this.Name.Length] != '(')
            {
                throw new ArgumentException(
                    "The function's opening bracket must be right after the name of the function!");
            }

            this.replaceStartIndex = indexOfFunctionName;

            // Removing the leading chars
            var trimmedExpression = this.Expression.Substring(indexOfFunctionName);

            var openingBracketIndex = trimmedExpression.IndexOf('(');
            var closingBracketIndex = this.FindClosingBracketIndex(trimmedExpression, openingBracketIndex);

            this.Argument = trimmedExpression.Substring(openingBracketIndex + 1, closingBracketIndex - openingBracketIndex - 1);
            this.replaceEndIndex = this.replaceStartIndex + closingBracketIndex  + 1;

            var value = NumericalParser.ParseDouble(this.Argument);

            if (value.HasValue)
            {
                this.ArgumentValue = value;
            }
            else
            {
                try
                {
                    this.InnerExpression = new CustomExpression(this.Argument);
                }
                catch (Exception)
                {
                    // Probably this is a normal math expression
                    this.ArgumentValue = Operations.Compute(this.Argument);
                }
            }
        }

        /// <summary>
        /// Gets the custom function expression.
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// Gets the custom function name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the custom function argument as text.
        /// </summary>
        public string Argument { get; private set; }

        /// <summary>
        /// Gets the custom function argument as double. The value is null, when the argument is another function.
        /// </summary>
        public double? ArgumentValue { get; private set; }

        /// <summary>
        /// Gets the inner function. This is null, when the function has a numeric argument.
        /// </summary>
        public CustomExpression InnerExpression { get; private set; }

        /// <summary>
        /// Pre-calculates the custom math expression and removes the custom function calls. 
        /// </summary>
        /// <returns>A simplified math expression.</returns>
        public string Simplify()
        {
            if (this.Name == null)
            {
                throw new InvalidOperationException(
                    "Can not compute the function, because its name is null.");
            }

            if (!Custom.Functions.ContainsKey(this.Name))
            {
                throw new InvalidOperationException(
                    "Undefined custom function: " + this.Name);
            }

            var children = this.GetChildren(this).ToList();
            if (children.Count == 0)
            {
                var result = Custom.Functions[this.Name].Invoke(this.ArgumentValue.Value);
                var leading = this.Expression.Substring(0, this.replaceStartIndex);
                var trailing = this.Expression.Substring(this.replaceEndIndex);

                return leading + result + trailing;
            }
            else
            {
                double? result = null;
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    var funct = children[i];
                    if (funct.ArgumentValue.HasValue)
                    {
                        result = Operations.Compute(funct.Simplify());
                    }
                    else
                    {
                        if (!result.HasValue)
                        {
                            continue;
                        }

                        result = Custom.Functions[funct.Name].Invoke(result.Value);
                    }
                }

                var finalResult = Custom.Functions[this.Name].Invoke(result.Value);

                var leading = this.Expression.Substring(0, this.replaceStartIndex);
                var trailing = this.Expression.Substring(this.replaceEndIndex);

                return leading + finalResult + trailing;
            }
        }

        /// <summary>
        /// Gets a list of the sub expressions.
        /// </summary>
        /// <param name="expression">The root expression.</param>
        /// <returns>List of the sub expressions</returns>
        private IEnumerable<CustomExpression> GetChildren(CustomExpression expression)
        {
            if (expression.InnerExpression != null)
            {
                yield return expression.InnerExpression;

                var children = this.GetChildren(expression.InnerExpression);
                foreach (var child in children)
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Finds the closing bracket index.
        /// </summary>
        /// <param name="input">The string, which contains the braces.</param>
        /// <param name="openBracketIndex">The target open bracket index.</param>
        /// <returns>The index of the matching bracket.</returns>
        /// <exception cref="InvalidProgramException">Thrown, when nothing is found.</exception>
        private int FindClosingBracketIndex(string input, int openBracketIndex)
        {
            Stack<int> openingBraces = new Stack<int>();
            var braces = new Dictionary<int, int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    openingBraces.Push(i);
                }
                else if (input[i] == ')')
                {
                    if (openingBraces.Count != 0)
                    {
                        braces.Add(openingBraces.Pop(), i);
                    }
                }
            }

            if (braces.ContainsKey(openBracketIndex))
            {
                return braces[openBracketIndex];
            }

            throw new InvalidProgramException("FindClosingBracketIndex method failed!");
        }
    }
}