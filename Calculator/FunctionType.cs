// <copyright file="FunctionType.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    /// <summary>
    /// Enumeration, which determines the <see cref="CalculatorButton"/>'s function.
    /// </summary>
    public enum FunctionType
    {
        /// <summary>
        /// Zero digit button.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// One digit button.
        /// </summary>
        One = 1,

        /// <summary>
        /// Two digit button.
        /// </summary>
        Two = 2,

        /// <summary>
        /// Three digit button.
        /// </summary>
        Three = 3,

        /// <summary>
        /// Four digit button.
        /// </summary>
        Four = 4,

        /// <summary>
        /// Five digit button.
        /// </summary>
        Five = 5,

        /// <summary>
        /// Six digit button.
        /// </summary>
        Six = 6,

        /// <summary>
        /// Seven digit button.
        /// </summary>
        Seven = 7,

        /// <summary>
        /// Eight digit button.
        /// </summary>
        Eight = 8,
        
        /// <summary>
        /// Nine digit button.
        /// </summary>
        Nine = 9,

        /// <summary>
        /// Plus operator.
        /// </summary>
        Plus = 10,

        /// <summary>
        /// Minus operator.
        /// </summary>
        Minus = 11,

        /// <summary>
        /// Multiply operator.
        /// </summary>
        Multiply = 12,

        /// <summary>
        /// Divide operator.
        /// </summary>
        Divide = 13,

        /// <summary>
        /// Comma sign.
        /// </summary>
        Comma = 14,

        /// <summary>
        /// Equals operation.
        /// </summary>
        Equals = 15,

        /// <summary>
        /// Back operation.
        /// </summary>
        Back = 16,

        /// <summary>
        /// Clear operation.
        /// </summary>
        Clear = 17,

        /// <summary>
        /// Clear entry operation.
        /// </summary>
        ClearEntry = 18,

        /// <summary>
        /// Left bracket.
        /// </summary>
        LeftBracket = 19,

        /// <summary>
        /// Right bracket.
        /// </summary>
        RightBracket = 20,

        /* Custom Functions must be defined in range 101-200 */

        /// <summary>
        /// Square root function.
        /// </summary>
        Sqrt = 101,

        /// <summary>
        /// Plus/Minus operation.
        /// </summary>
        Negate = 102,

        /// <summary>
        /// Reciprocal function.
        /// </summary>
        Reciproc = 103,

        /// <summary>
        /// Sine function, which expects degrees.
        /// </summary>
        Sind = 104,

        /// <summary>
        /// Cosine function, which expects degrees.
        /// </summary>
        Cosd = 105,

        /// <summary>
        /// Tangent function, which expect degrees.
        /// </summary>
        Tand = 106,

        /// <summary>
        /// Common logarithm with base 10.
        /// </summary>
        Log = 107,

        /// <summary>
        /// Function, which converts grads to degrees.
        /// </summary>
        Grad2Deg = 108,

        /// <summary>
        /// Function, which converts radians to degrees.
        /// </summary>
        Rad2Deg = 109,

        /* Custom operators must be defined in range 201-300 */

        /// <summary>
        /// Power operator.
        /// </summary>
        Power = 201,

        /// <summary>
        /// PI Constant.
        /// </summary>
        Pi = 301,

        /// <summary>
        /// E Constant.
        /// </summary>
        E = 302,
    }
}