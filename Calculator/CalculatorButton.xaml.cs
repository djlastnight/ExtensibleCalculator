// <copyright file="CalculatorButton.xaml.cs" company="Ivan Yankov">
//     Copyright (c) Ivan Yankov 2018. All rights reserved.
// </copyright>
// <summary>.</summary>
// <author>Ivan Yankov</author>
namespace Calculator
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for CalculatorButton.
    /// </summary>
    public partial class CalculatorButton : UserControl
    {
        /// <summary>
        /// Determines the Calculator button's function.
        /// </summary>
        public static readonly DependencyProperty FunctionTypeProperty =
            DependencyProperty.Register(
            "FunctionType",
            typeof(FunctionType),
            typeof(CalculatorButton),
            new PropertyMetadata(FunctionType.Zero));

        /// <summary>
        /// Determines the button content, which is a text.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(CalculatorButton),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// IValueConverter, used for bindings between the function type and the text.
        /// </summary>
        private FunctionTypeToTextValueConverter converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorButton"/> class.
        /// </summary>
        public CalculatorButton()
        {
            this.InitializeComponent();
            this.converter = new FunctionTypeToTextValueConverter();

            // Setting binding between FunctionType and Text dependency properties, using a custom converter.
            var binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath(CalculatorButton.FunctionTypeProperty);
            binding.Converter = this.converter;
            binding.ConverterParameter = null;
            binding.ConverterCulture = CultureInfo.CurrentCulture;
            BindingOperations.SetBinding(this, CalculatorButton.TextProperty, binding);
        }

        /// <summary>
        /// Gets or sets the <see cref="CalculatorButton"/>'s function.
        /// </summary>
        public FunctionType FunctionType
        {
            get { return (FunctionType)this.GetValue(FunctionTypeProperty); }
            set { this.SetValue(FunctionTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="CalculatorButton"/>'s text.
        /// </summary>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the calculator button is a math operator ('+', '-', '*' or '/').
        /// </summary>
        public bool IsOperatorButton
        {
            get
            {
                int value = (int)this.FunctionType;
                bool isCustomOperator = value > 200 && value <= 300;

                return isCustomOperator ||
                    this.FunctionType == Calculator.FunctionType.Plus ||
                    this.FunctionType == Calculator.FunctionType.Minus ||
                    this.FunctionType == Calculator.FunctionType.Multiply ||
                    this.FunctionType == Calculator.FunctionType.Divide;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the calculator button is a digit button (0-9).
        /// </summary>
        public bool IsDigitButton
        {
            get
            {
                return (int)this.FunctionType < 10;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the button is left or right bracket button.
        /// </summary>
        public bool IsBracket
        {
            get
            {
                return this.FunctionType == Calculator.FunctionType.LeftBracket ||
                    this.FunctionType == Calculator.FunctionType.RightBracket;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the calculator button allows corrections.
        /// </summary>
        public bool IsErasableButton
        {
            get
            {
                return this.IsDigitButton ||
                    this.IsBracket ||
                    this.FunctionType == Calculator.FunctionType.Comma ||
                    this.FunctionType == Calculator.FunctionType.Back;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the calculator button represents a custom function.
        /// </summary>
        public bool IsCustomFunction
        {
            get
            {
                int value = (int)this.FunctionType;
                return value > 100 && value <= 200;
            }
        }
    }
}
