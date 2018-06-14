// <copyright file="FunctionTypeToTextValueConverter.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="CalculatorButton"/>'s FunctionType to math operation string.
    /// </summary>
    public class FunctionTypeToTextValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts <see cref="CalculatorButton"/>'s FunctionType to math operation string.
        /// </summary>
        /// <param name="value">The input value to convert. Expected type is FunctionType enumeration.</param>
        /// <param name="targetType">The target type, which must be string type.</param>
        /// <param name="parameter">The converter's parameter. Pass null.</param>
        /// <param name="culture">The converter's culture. Pass the current culture.</param>
        /// <returns>Returns math operator as string.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is FunctionType && targetType == typeof(string))
            {
                var function = (FunctionType)value;
                switch (function)
                {
                    case FunctionType.Zero:
                        return "0";
                    case FunctionType.One:
                        return "1";
                    case FunctionType.Two:
                        return "2";
                    case FunctionType.Three:
                        return "3";
                    case FunctionType.Four:
                        return "4";
                    case FunctionType.Five:
                        return "5";
                    case FunctionType.Six:
                        return "6";
                    case FunctionType.Seven:
                        return "7";
                    case FunctionType.Eight:
                        return "8";
                    case FunctionType.Nine:
                        return "9";
                    case FunctionType.Plus:
                        return "+";
                    case FunctionType.Minus:
                        return "-";
                    case FunctionType.Multiply:
                        return "*";
                    case FunctionType.Divide:
                        return "/";
                    case FunctionType.Power:
                        return "^";
                    case FunctionType.Comma:
                        return culture.NumberFormat.NumberDecimalSeparator;
                    case FunctionType.Equals:
                        return "=";
                    case FunctionType.Back:
                        return "←";
                    case FunctionType.Clear:
                        return "C";
                    case FunctionType.ClearEntry:
                        return "CE";
                    case FunctionType.Negate:
                        return "±";
                    case FunctionType.LeftBracket:
                        return "(";
                    case FunctionType.RightBracket:
                        return ")";
                    case FunctionType.Sqrt:
                        return "√";
                    case FunctionType.Reciproc:
                        return "⅟ᵪ";
                    case FunctionType.Sind:
                        return "sin";
                    case FunctionType.Cosd:
                        return "cos";
                    case FunctionType.Tand:
                        return "tan";
                    case FunctionType.Log:
                        return "log";
                    case FunctionType.Grad2Deg:
                        return "G->D";
                    case FunctionType.Rad2Deg:
                        return "R->D";
                    case FunctionType.Pi:
                        return "π";
                    case FunctionType.E:
                        return "E";
                    default:
                        throw new NotImplementedException("Not implemented function: " + function);
                }
            }

            return value;
        }

        /// <summary>
        /// Not implemented method.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type used for the conversion.</param>
        /// <param name="parameter">The converter's parameter.</param>
        /// <param name="culture">The converter's culture.</param>
        /// <returns>Returns the converter value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
