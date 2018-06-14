// <copyright file="RelayCommand.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System;
    using System.Windows.Input;
    
    /// <summary>
    /// The execute delegate.
    /// </summary>
    /// <param name="parameter">The delegate parameter of object type.</param>
    public delegate void ExecuteDelegate(object parameter);

    /// <summary>
    /// The can execute delegate.
    /// </summary>
    /// <param name="parameter">The delegate parameter of object type.</param>
    /// <returns>Returns true if the operation can be executed.</returns>
    public delegate bool CanExecuteDelegate(object parameter);

    /// <summary>
    /// <see cref="RelayCommand"/> class, which implements <see cref="ICommand"/> interface.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// A reference to a Execute delegate.
        /// </summary>
        private ExecuteDelegate execute;

        /// <summary>
        /// A reference to a CanExecute delegate.
        /// </summary>
        private CanExecuteDelegate canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method, which will be called when the command is executed.</param>
        public RelayCommand(ExecuteDelegate execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method, which will be called when the command is executed.</param>
        /// <param name="canExecute">The method, which determines whether the command could be executed.</param>
        public RelayCommand(ExecuteDelegate execute, CanExecuteDelegate canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// <see cref="CanExecuteChanged"/> event. Fired when the <see cref="CanExecuteDelegate"/> value has been changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Checks whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The can execute parameter of object type.</param>
        /// <returns>Returns true, if the command can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            return this.canExecute(parameter);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">A parameter of object type passed along with the command.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}