// <copyright file="MainWindow.xaml.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The exit command for <see cref="Window"/>.
        /// </summary>
        public static readonly DependencyProperty ExitCommandProperty =
            DependencyProperty.Register(
            "ExitCommand",
            typeof(ICommand),
            typeof(MainWindow),
            new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.ExitCommand = new RelayCommand(this.OnExitCommandRequested);
        }

        /// <summary>
        /// Gets or sets the exit command.
        /// </summary>
        public ICommand ExitCommand
        {
            get { return (ICommand)this.GetValue(ExitCommandProperty); }
            set { this.SetValue(ExitCommandProperty, value); }
        }

        /// <summary>
        /// The exit command delegate.
        /// </summary>
        /// <param name="parameter">Parameter of object type. In this case - always null.</param>
        private void OnExitCommandRequested(object parameter)
        {
            App.Current.Shutdown(0);
        }
    }
}
