// <copyright file="VisualHelper.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Visual helper class. Provides methods for searching visual elements.
    /// </summary>
    public static class VisualHelper
    {
        /// <summary>
        /// Finds a calculator button of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">The dependency object parent.</param>
        /// <param name="content">The buttons' content.</param>
        /// <returns>Null if nothing found.</returns>
        public static CalculatorButton FindButton(DependencyObject parent, string content)
        {
            // Confirm parent and childName are valid. 
            if (parent == null)
            {
                return null;
            }

            CalculatorButton foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is not of the request child type child
                CalculatorButton childType = child as CalculatorButton;
                if (childType == null)
                {
                    // Recursively drill down the tree
                    foundChild = FindButton(child, content);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(content))
                {
                    var button = child as CalculatorButton;

                    // If the button text is set for search
                    if (button != null && button.Text == content)
                    {
                        // If the child's name is of the request name
                        foundChild = (CalculatorButton)child;
                        break;
                    }
                }
                else
                {
                    // Child element found.
                    foundChild = (CalculatorButton)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
