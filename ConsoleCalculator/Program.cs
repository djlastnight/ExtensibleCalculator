// <copyright file="Program.cs" company="Ivan Yankov">
// Copyright (c) Ivan Yankov. All rights reserved.
// </copyright>
// <author>Ivan Yankov</author>
namespace ConsoleCalculator
{
    using System;

    /// <summary>
    /// The console app main class.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The console app's entry point.
        /// </summary>
        /// <param name="args">The arguments passed to the app.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Please write math expression to calc.");
            string message = string.Empty;
            while ((message = Console.ReadLine()).Length > 0)
            {
                Console.WriteLine(CalculatorCore.Operations.Compute(message));
            }
        }
    }
}
