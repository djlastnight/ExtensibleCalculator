// <copyright file="InputBindingBehavior.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Allows input bindings redirection from any <see cref="FrameworkElement"/> to its parent <see cref="Window"/> to avoid 'must be focused' bug.
    /// </summary>
    public class InputBindingBehavior
    {
        /// <summary>
        /// The propagate input bindings attached property.
        /// </summary>
        public static readonly DependencyProperty PropagateInputBindingsToWindowProperty =
            DependencyProperty.RegisterAttached(
            "PropagateInputBindingsToWindow",
            typeof(bool),
            typeof(InputBindingBehavior),
            new PropertyMetadata(false, OnPropagateInputBindingsToWindowChanged));

        /// <summary>
        /// Gets a value indicating whether the propagate behavior is currently set for a passed <see cref="FrameworkElement"/>
        /// </summary>
        /// <param name="obj">The associated <see cref="FrameworkElement"/>.</param>
        /// <returns>Returns true if the behavior is attached.</returns>
        public static bool GetPropagateInputBindingsToWindow(FrameworkElement obj)
        {
            return (bool)obj.GetValue(PropagateInputBindingsToWindowProperty);
        }

        /// <summary>
        /// Sets the propagate behavior for a passed <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to set.</param>
        /// <param name="value">The new boolean value.</param>
        public static void SetPropagateInputBindingsToWindow(FrameworkElement obj, bool value)
        {
            obj.SetValue(PropagateInputBindingsToWindowProperty, value);
        }

        /// <summary>
        /// Event handled used to set the Loaded event.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The event args.</param>
        private static void OnPropagateInputBindingsToWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FrameworkElement)d).Loaded += OnFrameworkElementLoaded;
        }

        /// <summary>
        /// Event handler, which moves the input bindings from <see cref="FrameworkElement"/> to its parent <see cref="Window"/>.
        /// </summary>
        /// <param name="sender">The framework element.</param>
        /// <param name="e">The event args.</param>
        private static void OnFrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;
            frameworkElement.Loaded -= OnFrameworkElementLoaded;

            var window = Window.GetWindow(frameworkElement);
            if (window == null)
            {
                return;
            }

            // Move input bindings from the FrameworkElement to the window.
            for (int i = frameworkElement.InputBindings.Count - 1; i >= 0; i--)
            {
                var inputBinding = (InputBinding)frameworkElement.InputBindings[i];
                window.InputBindings.Add(inputBinding);
                frameworkElement.InputBindings.Remove(inputBinding);
            }
        }
    }
}
